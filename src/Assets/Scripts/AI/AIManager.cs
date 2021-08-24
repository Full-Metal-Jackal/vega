using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Inventory;


namespace AI
{
	public class AIManager : MobController
	{
		public AIState currentState;
		public Mob currentTarget;
		public CoverSpot currentCover;

		public bool CanSeeTarget { get; private set; }
		public Mob Player { get; private set; }
		private Mob mob;
		private float currentRecoveryTime = 0;

		[HideInInspector]
		public NavMeshPathVisualizer navMeshVisualizer;
		[HideInInspector]
		public bool isPerfomingAction;
		[HideInInspector]
		public float currentMovementRecoveryTime = 0;
		[HideInInspector]
		public float distanceFromTarget;
		[HideInInspector]
		public bool InCover = false;
		private const float rangeCoefficient = 0.9f;
		public float StoppingDistance { get; protected set; }

		public float CurrentRecoveryTime 
		{ 
			get => currentRecoveryTime;
			set => currentRecoveryTime = value; 
		}

		[Header("A.I Settings")]
		public ItemData weaponData;
		public NavMeshAgent navMeshAgent;
		public float detectionRadius = 5;
		public float maxDetectionAngle = 50;
		public float minDetectionAngle = -50;
		public float viewableAngle;
		public float rotationSpeed = 15;
		public float maxAttackRange = 1.5f;
		public float maxMovementRecoveryTime = 5;
		public LayerMask detectionLayer;
		public LayerMask coverSpotsLayer;
		public AIAttackAction[] aiAttacks;

		protected override void Initialize()
		{
			base.Initialize();
			Player = PlayerController.Instance.possessAtStart;
			navMeshAgent = transform.parent.GetComponentInChildren<NavMeshAgent>();
			navMeshAgent.updateRotation = false;
			navMeshAgent.enabled = false;
			navMeshVisualizer = transform.parent.GetComponentInChildren<NavMeshPathVisualizer>();
		}

		protected override void Setup()
		{
			base.Setup();
			mob = Possessed;
			AssignWeapon();
			StoppingDistance = maxAttackRange * rangeCoefficient;
		}

		protected override void OnUpdate(float delta)
		{
			CheckTargetVisibility();
			CheckIfInCover();
			HandleStateMachine(delta);
			HandleRecoveryTime(delta);
		}

		private void HandleStateMachine(float delta)
		{
			if (currentState != null)
			{
				AIState nextState = currentState.Tick(this, mob);

				if (nextState != null)
				{
					SwitchToNextState(nextState);
				}
			}
		}

		private void SwitchToNextState(AIState state)
		{
			currentState = state;
		}

		private void HandleRecoveryTime(float delta)
		{
			if (currentRecoveryTime > 0)
			{
				currentRecoveryTime -= delta;
			}
			else if (isPerfomingAction)
			{
				isPerfomingAction = false;
			}

			if (currentMovementRecoveryTime > 0)
			{
				currentMovementRecoveryTime -= delta;
			}
		}

		private void AssignWeapon()
		{
			if (!(weaponData.PasteItem(Containers.Instance.Items) is Gun weapon))
			{
				Debug.Log("Weapon assign failed");
				return;
			}
			mob.PickUpItem(weapon);
		}

		private void CheckTargetVisibility()
		{
			if (currentTarget == null)
			{
				CanSeeTarget = false;
				return;
			}
			Vector3 castFrom = transform.position + Vector3.up * mob.AimHeight;
			Vector3 castTo = currentTarget.transform.position + Vector3.up * mob.AimHeight - castFrom;
			if (Physics.Raycast(castFrom, castTo, out RaycastHit hit, detectionRadius))
			{
				var detection = hit.transform;
				if (detection.GetComponent<Mob>() == Player)
				{
					CanSeeTarget = true;
				}
				else
				{
					CanSeeTarget = false;
				}	
			}
		}

		public bool FindCover(out CoverSpot cover)
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, coverSpotsLayer);
			foreach (Collider colliderElem in colliders)
			{
				if (colliderElem.TryGetComponent<CoverSpot>(out cover))
				{
					if (!cover.IsOccupied && !cover.IsDestroyed)
					{
						//TODO Добавить проверку на дальность
						return true;
					}
				}
			}
			cover = null;
			return false;
		}

		private void CheckIfInCover()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, coverSpotsLayer);
			foreach (Collider colliderElem in colliders)
			{
				if (colliderElem.TryGetComponent<CoverSpot>(out CoverSpot cover))
				{
					if (cover == currentCover)
					{
						InCover = true;
						return;
					}
				}
			}
			InCover = false;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, 0.5f);
		}
	}
}
