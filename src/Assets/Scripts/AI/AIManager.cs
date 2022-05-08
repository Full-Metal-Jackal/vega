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
		public float currentDashRecoveryTime = 0;
		[HideInInspector]
		public float distanceFromTarget;
		[HideInInspector]
		public bool inCover = false;
		//[HideInInspector]
		public AIState currentState;
		[HideInInspector]
		public Mob currentTarget;
		[HideInInspector]
		public CoverSpot currentCover;
		//[HideInInspector]
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

		public Vector3 TargetPos => currentTarget.transform.position + currentTarget.AimHeight * Vector3.up;
		public Vector3 DefaultTargetDirection => currentTarget.transform.position - mob.transform.position;

		[field: SerializeField]
		public ItemData StartItemData { get; private set; }

		[field: SerializeField]
		public float DetectionRadius { get; private set; }
		[field: SerializeField]
		public float DangerThreshhold { get; private set; }
		[field: SerializeField]
		public float MaxAttackRange { get; private set; }
		[field: SerializeField]
		public float MaxMovementRecoveryTime { get; private set; }
		[field: SerializeField]
		public float DashRecoveryTime { get; private set; }
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

		public GameObject DebugCube;
		public GameObject DebugCube2;

		protected override void Awake()
		{
			base.Awake();

			NavMeshAgent = transform.GetComponentInChildren<NavMeshAgent>();
			NavMeshObstacle = transform.GetComponentInChildren<NavMeshObstacle>();
			NavMeshAgent.updateRotation = false;
			NavMeshAgent.enabled = false;
			NavMeshObstacle.enabled = true;

			StoppingDistance = MaxAttackRange * rangeCoefficient;

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

			if (currentDashRecoveryTime > 0)
			{
				currentDashRecoveryTime -= delta;
			}

			if (patternRecoveryTime > 0)
			{
				patternRecoveryTime -= delta;
			}
		}

		public void PerfomeAction(float time)
		{
			StartCoroutine(WaitFor(time));
		}

		private IEnumerator WaitFor(float waitTime)
		{
			isPerfomingAction = true;
			yield return new WaitForSeconds(waitTime);
			isPerfomingAction = false;
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
			Vector3 castFrom = mob.transform.position + Vector3.up * mob.AimHeight;
			Vector3 castDir = TargetPos - castFrom;

			//DebugCube.transform.position = castTo;
			//DebugCube2.transform.position = castFrom;

			if (Physics.Raycast(castFrom, castDir, out RaycastHit hit, DetectionRadius, obstacleLayer))
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
			Collider[] colliders = Physics.OverlapSphere(transform.position, DetectionRadius, CoverSpotsLayer);
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
			Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, CoverSpotsLayer);
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
			Gizmos.DrawWireSphere(transform.position, DetectionRadius);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, 0.5f);

			if (mob)
			{
				Vector3 castFrom = transform.position + Vector3.up * mob.AimHeight;
				Vector3 castTo = TargetPos;
				Gizmos.color = Color.red;
				Gizmos.DrawLine(castFrom, castTo);
			}

		}
	}
}
