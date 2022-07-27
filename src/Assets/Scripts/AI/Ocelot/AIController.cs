using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor;

namespace OcelotAI
{
	public class AIController : MobController
	{
		private readonly float tickDelay = .5f;
		private float nextTick = 0f;

		private readonly float visionDistance = 100f;
		private int visionMask;

		private readonly float detectionRadius = 100f;
		private Collider[] detectionBuffer = new Collider[16];
		private int detectionMask;
		private int enemiesAround = 0;

		private int mobMask;

		public Mob Target { get; protected set; }

		public Gun Gun { get; protected set; }

		[SerializeField]
		private NavMeshAgent agent;

		private NavMeshPath path;

		[field: SerializeField]
		protected AIStateMachine StateMachine { get; private set; }

		private readonly float visibilityScore = 1f;
		private readonly float persistenceScore = 1f;
		private readonly float scorePerMeter = .01f;
		private readonly float scorePerHealthDifference = .1f;

		private readonly float threatPerDamageRisk = .1f;
		private readonly float threatPerHitRisk = .01f;

		public bool CanShootTarget => HasClearShot(Target);
		public bool CanSeeTarget => CanSee(Target);

		protected override void Awake()
		{
			base.Awake();

			visionMask = Utils.CreateMask(new Layer[]
				{ Layer.Mobs, Layer.Default, Layer.Obstacles }
			);

			detectionMask = Utils.CreateMask(new Layer[]
				{ Layer.Mobs }
			);

			mobMask = Utils.CreateMask(new Layer[]
				{ Layer.MobObstacle, Layer.Default, Layer.Obstacles }
			);

			path = new NavMeshPath();
		}

		protected override void Start()
		{
			base.Start();

			Debug.Assert(
				StateMachine != null && StateMachine.Controller == this,
				$"{this} has invalid two-way link with its state machine."
			);
			{
				foreach (AIBehaviour behaviour in StateMachine.Behaviours)
				{
					behaviour.OnAim += InputAimPos;
					behaviour.OnTriggerStateChanged += InputTrigger;
					behaviour.OnThrow += InputThrow;

					behaviour.OnMoveDirect += InputMove;
					behaviour.OnMoveTo += MoveTo;

					behaviour.OnDodge += InputDodge;
				}
			}
		}

		public void InputAimPos(Vector3 pos) =>
			Possessed.AimPos = pos;

		public override void PossessMob(Mob mob)
		{
			if (Possessed != null)
			{
				// <TODO> Unlink Possessed.OnDamaged here
				mob.OnActiveItemChanged -= UpdateActiveItem;
			}

			base.PossessMob(mob);

			if (mob.Faction != Faction.Hostile)
				Debug.LogWarning($"Hostile AI assigned to {mob} with non-hostile faction.");

			agent.speed = Possessed.MoveSpeed;
			agent.angularSpeed = Possessed.Body.maxAngularVelocity;

			mob.OnDamaged += (_) => UpdateTarget();

			UpdateActiveItem(mob.ActiveItem);
			mob.OnActiveItemChanged += UpdateActiveItem;

			UpdateTarget();
			UpdateStateMachine();
		}

		// <TODO> This wrapper was used to pass context. May be repurposed later.
		protected virtual void UpdateStateMachine() => StateMachine.UpdateBehaviour();

		protected override void Update()
		{
			if (!Possessed)
				return;
			
			if (Time.time > nextTick)
			{
				nextTick = Time.time + tickDelay;
				Tick();
			}
		}

		/// <summary>
		/// Tick is responsible for selecting current enemy and attack pattern.
		/// </summary>
		protected virtual void Tick()
		{
			UpdateTarget();
			UpdateStateMachine();
		}

		protected virtual void UpdateTarget()
		{
			enemiesAround = 0;

			int found = Physics.OverlapSphereNonAlloc(
				Possessed.transform.position,
				detectionRadius, detectionBuffer, detectionMask
			);

			if (found == 0)
				return;

			Mob newTarget = null;
			float targetPriority = float.MinValue;

			for (int i = 0; i < found; i++)
			{
				Collider collider = detectionBuffer[i];
				if (!collider.transform.parent.TryGetComponent(out Mob mob))
				{
					Debug.LogWarning($"{collider} is assigned to \"Mobs\" layer but has no mob parent.");
					continue;
				}

				if (!IsEnemy(mob))
					continue;

				enemiesAround++;

				float priority = GetEnemyPriority(mob);
				if (priority <= targetPriority)
					continue;

				targetPriority = priority;
				newTarget = mob;
			}

			Target = newTarget;
		}

		/// <summary>
		/// Updates all properties referencing the mob's current item.
		/// It's more efficient to do this only when the item changes than on every AI tick.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void UpdateActiveItem(Inventory.Item item)
		{
			Gun = item as Gun;
		}

		protected virtual void MoveTo(Vector3 pos)
		{
			Vector3 movement = Vector3.zero;


			if (agent.CalculatePath(pos, path) && path.corners.Length >= 2)
			{
				movement = path.corners[1] - path.corners[0];
				
				const float cornerEvasionDistance = 1f;
				movement += movement.normalized * cornerEvasionDistance;
			}

			InputMove(movement);
		}

		/// <summary>
		/// Assesses attack priority of a mob.
		/// The higher the return value, the more likely the target is to be attacked by the possessed mob.
		/// </summary>
		/// <returns>Priority of the mob as target.</returns>
		protected virtual float GetEnemyPriority(Mob target)
		{
			float score = 0f;

			// The mob is more likely to stick to its current target.
			if (target == Target)
				score += persistenceScore;

			// The mob is more likely to attack visible targets.
			if (CanSee(target))
				score += visibilityScore;

			// The mob is less likely to attack distant targets.
			score -= Vector3.Distance(target.transform.position, Possessed.transform.position) * scorePerMeter;

			// The mob is more likely to finish low-HP targets.
			score += (Possessed.Health - target.Health) * scorePerHealthDifference;

			return score;
		}

		protected virtual float GetEnemyThreatLevel(Mob target)
		{
			float threat = 0f;

			if (target.ActiveItem is Gun gun)
			{
				float damageRisk = gun.Damage.amount / Possessed.Health;
				
				// Weapons with extremely low fire rate should be considered as dangerous, hence the Mahtf.Max call.
				const float secsPerMinute = 60f;
				damageRisk *= Mathf.Max(gun.FireRate / secsPerMinute, 1);
				
				float hitRisk = Vector3.Distance(
					target.transform.position,
					Possessed.transform.position
				) / gun.ProjectileSpeed;

				threat += damageRisk * threatPerDamageRisk;
				threat += hitRisk * threatPerHitRisk;
			}


			return threat;
		}

		public virtual bool CanSee(Mob target)
		{
			if (target == null)
				return false;

			Vector3 origin = Possessed.AimOrigin;

			Physics.Raycast(
				origin, target.AimOrigin - origin, out RaycastHit hit,
				visionDistance, visionMask
			);

			return hit.rigidbody == target.Body;
		}

		public virtual bool HasClearShot(Mob target)
		{
			if (target == null || !(Possessed.ActiveItem is Gun gun))
				return false;
			
			Vector3 origin = gun.Barrel.position;

			Physics.Raycast(
				origin, target.AimOrigin - origin, out RaycastHit hit,
				visionDistance, visionMask
			);

			return hit.rigidbody == target.Body;
		}

		public virtual Vector3 InterceptionShotPosition(
			Mob target,
			bool keepY = false,
			float projectileFlightCorrection = 1f,
			float distanceEstimationCorrection = .2f
		)
		{
			Vector3 result = target.AimOrigin;
			
			if (!(Possessed.ActiveItem is Gun gun))
			{
				Debug.LogWarning($"{this} attempted interception shot without a gun.");
				return result;
			}

			Vector3 projectileSource = gun.Barrel.position;
			Vector3 velocity = target.Body.velocity;

			float immedieateDistance = Vector3.Distance(projectileSource, result);
			float estimatedDistance = Vector3.Distance(
				projectileSource,
				result + velocity * immedieateDistance * distanceEstimationCorrection
			);
			
			float projectileFlightTime = estimatedDistance / gun.ProjectileSpeed;

			if (
				Physics.Raycast(
					result, velocity, out RaycastHit hit,
					velocity.magnitude * projectileFlightTime, mobMask
				)
			)
				result = hit.point;
			else
				result += projectileFlightCorrection * projectileFlightTime * velocity;

			if (keepY)
				result.y = projectileSource.y;

			return result;
		}

		protected virtual bool IsEnemy(Mob mob)
		{
			return mob.Faction == Faction.Player && mob.Alive;
		}

		private void OnDestroy()
		{
			foreach (AIBehaviour behaviour in StateMachine.Behaviours)
			{
				behaviour.OnAim -= InputAimPos;
				behaviour.OnTriggerStateChanged -= InputTrigger;
				behaviour.OnThrow -= InputThrow;

				behaviour.OnMoveDirect -= InputMove;
				behaviour.OnMoveTo -= MoveTo;

				behaviour.OnDodge -= InputDodge;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Handles.color = Color.red;
			Handles.Label(transform.position, $"Target: {Target}\n"+
				$"Behaviour: {StateMachine.ActiveBehaviour}");
		}
	}
}
