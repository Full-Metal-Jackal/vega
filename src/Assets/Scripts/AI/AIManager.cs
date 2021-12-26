using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Inventory;


namespace AI
{
	public class AIManager : MobController
	{
		public bool CanSeeTarget { get; private set; }
		private Mob mob;
		private float currentRecoveryTime = 0;
		private float patternRecoveryTime = 0;


		[HideInInspector]
		public bool isPerfomingAction;
		[HideInInspector]
		public float currentMovementRecoveryTime = 0;
		[HideInInspector]
		public float distanceFromTarget;
		[HideInInspector]
		public bool inCover = false;
		[HideInInspector]
		public AIState currentState;
		[HideInInspector]
		public Mob currentTarget;
		[HideInInspector]
		public CoverSpot currentCover;
		[HideInInspector]
		public CombatPattern currentPattern;

		private int obstacleLayer;

		private const float rangeCoefficient = 0.5f;
		public float StoppingDistance { get; protected set; }

		public float CurrentRecoveryTime 
		{ 
			get => currentRecoveryTime;
			set => currentRecoveryTime = value; 
		}

		public float PatternRecoveryTime
		{
			get => patternRecoveryTime;
			set => patternRecoveryTime = value;
		}

		[field: SerializeField]
		public ItemData StartItemData { get; private set; }

		[Header("A.I Settings")]
		private float detectionRadius = 5;
		private float dangerThreshhold = 2.5f;
		private float maxAttackRange = 5;
		private float maxMovementRecoveryTime = 2;
		private float agressiveMovementRecoveryTime = 1;
		private float shootingRecoveryTime = 1;
		private float agressiveShootingRecoveryTime = 0.5f;
		private LayerMask detectionLayer;
		private LayerMask coverSpotsLayer;
		[field: SerializeField]
		public float DetectionRadius { get; private set; }
		[field: SerializeField]
		public float DangerThreshhold { get; private set; }
		[field: SerializeField]
		public float MaxAttackRange { get; private set; }
		[field: SerializeField]
		public float MaxMovementRecoveryTime { get; private set; }
		[field: SerializeField]
		public float AgressiveMovementRecoveryTime { get; private set; }
		[field: SerializeField]
		public float ShootingRecoveryTime { get; private set; }
		[field: SerializeField]
		public float AgressiveShootingRecoveryTime { get; private set; }
		[field: SerializeField]
		public LayerMask DetectionLayer { get; private set; }
		[field: SerializeField]
		public LayerMask CoverSpotsLayer { get; private set; }
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		public NavMeshObstacle NavMeshObstacle { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			NavMeshAgent = transform.GetComponentInChildren<NavMeshAgent>();
			NavMeshObstacle = transform.GetComponentInChildren<NavMeshObstacle>();
			NavMeshAgent.updateRotation = false;
			NavMeshAgent.enabled = false;
			NavMeshObstacle.enabled = true;

			StoppingDistance = maxAttackRange * rangeCoefficient;

			obstacleLayer = (1 << LayerMask.NameToLayer("Obstacles")) 
				| (1 << LayerMask.NameToLayer("Covers")) 
				| (1 << LayerMask.NameToLayer("Mobs")) 
				| (1 << LayerMask.NameToLayer("NavMeshDynamic"));
		}

		protected override void Start()
		{
			base.Start();

			mob = Possessed;
			
			GiveStartItem();
		}

		protected override void OnUpdate(float delta)
		{
			HandleTargetRelevance();
			CheckTargetVisibility();
			//CheckIfInCover();
			HandleStateMachine(delta);
			HandlePattern(delta);
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

		private void HandlePattern(float delta)
		{
			if (currentPattern != null)
			{
				currentPattern.Tick(this, mob);
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
			
			if (patternRecoveryTime > 0)
			{
				patternRecoveryTime -= delta;
			}
		}

		private void GiveStartItem()
		{
			if (!StartItemData)
				return;

			if (!(StartItemData.PasteItem(Containers.Instance.Items) is Item item))
				throw new System.Exception($"Invalid item data assigned for {mob}");

			mob.PickUpItem(item);
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
			if (Physics.Raycast(castFrom, castTo, out RaycastHit hit, detectionRadius, obstacleLayer))
			{
				Transform detection = hit.transform;
				if (detection.TryGetComponent(out Mob mob) && mob.Faction == Faction.Player)
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
					if (!cover.IsOccupied && cover.isSafe)
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
						inCover = true;
						return;
					}
				}
			}
			inCover = false;
		}

		public bool IsCurrentCoverRelevant()
		{
			if (!currentCover.isSafe || (currentCover.currentUser != mob && currentCover.IsOccupied))
			{
				inCover = false;
				currentCover = null;
				return false;
			}	
			return true;
		}

		private void HandleTargetRelevance()
		{
			if (currentTarget && !currentTarget.Alive)
			{
				currentTarget = null;
			}
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
