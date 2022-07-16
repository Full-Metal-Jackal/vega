using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OcelotAI
{
	public class SimpleHostileController : MobController
	{
		private readonly float tickDelay = .5f;
		private float nextTick = 0f;

		private readonly float visionDistance = 100f;
		private int visionMask;

		private readonly float detectionRadius = 100f;
		private Collider[] detectionBuffer = new Collider[16];
		private int detectionMask;

		private Mob currentTarget;

		[SerializeField]
		private NavMeshAgent agent;
		private NavMeshPath path;

		private AIStage currentStage;
		private AIPattern currentPattern;

		private readonly float visibilityScore = 1f;
		private readonly float persistenceScore = 1f;
		private readonly float scorePerMeter = .01f;
		private readonly float scorePerHealthDifference = .1f;

		private readonly float threatPerDamageRisk = .1f;
		private readonly float threatPerHitRisk = .01f;

		protected override void Awake()
		{
			base.Awake();

			visionMask = Utils.CreateMask(new Layer[]
				{ Layer.Mobs, Layer.Default, Layer.Obstacles}
			);

			detectionMask = Utils.CreateMask(new Layer[]
				{ Layer.Mobs }
			);

			path = new NavMeshPath();
		}

		public override void PossessMob(Mob mob)
		{
			base.PossessMob(mob);

			if (mob.Faction != Faction.Hostile)
				Debug.LogWarning($"Hostile AI assigned to {mob} with non-hostile faction.");

			agent.speed = Possessed.MoveSpeed;
			agent.angularSpeed = Possessed.Body.maxAngularVelocity;

			mob.OnDamaged += (_) => UpdateCurrentEnemy();
			
			UpdateCurrentEnemy();
			UpdateCurrentStage();
		}

		protected override void OnUpdate(float delta)
		{
			if (Time.time > nextTick)
			{
				nextTick = Time.time + tickDelay;
				Tick();
			}

			if (currentTarget)
			{
				Possessed.AimPos = currentTarget.AimOrigin;
		
				MoveTo(currentTarget.transform.position);
			}
		}

		/// <summary>
		/// Tick is responsible for selecting current enemy and attack pattern.
		/// </summary>
		protected virtual void Tick()
		{
			UpdateCurrentEnemy();

		}

		protected virtual void UpdateCurrentEnemy()
		{
			int found = Physics.OverlapSphereNonAlloc(
				Possessed.transform.position,
				detectionRadius, detectionBuffer, detectionMask
			);

			if (found == 0)
				return;

			Mob newTarget = null;
			float targetPriority = 0f;

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

				float priority = GetEnemyPriority(mob);
				if (priority <= targetPriority)
					continue;

				targetPriority = priority;
				newTarget = mob;
			}

			currentTarget = newTarget;
		}

		protected virtual void UpdateCurrentStage()
		{

		}

		protected virtual void Attack(Mob mob)
		{
			Possessed.UseItem(true);
			Possessed.UseItem(false);
		}

		protected virtual void MoveTo(Vector3 pos)
		{
			Vector3 newMovement = Vector3.zero;


			if (agent.CalculatePath(pos, path) && path.corners.Length >= 2)
			{
				newMovement = path.corners[1] - path.corners[0];
				
				const float cornerEvasionDistance = 1f;
				newMovement += newMovement.normalized * cornerEvasionDistance;
			}

			movement = newMovement;
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
			if (target == currentTarget)
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

		protected virtual bool CanSee(Mob target)
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

		protected virtual bool IsEnemy(Mob mob)
		{
			return mob.Faction == Faction.Player && mob.Alive;
		}

		private void OnDrawGizmos()
		{
		}
	}
}
