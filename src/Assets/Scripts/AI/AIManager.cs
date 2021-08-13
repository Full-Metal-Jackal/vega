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
		private bool isPerfomingAction;
		public bool IsPerfomingAction 
		{
			get
			{
				return isPerfomingAction;
			}

			set
			{
				isPerfomingAction = value;
			}
		}
		public Mob currentTarget;
		public Mob Player { get; private set; }  //needs to prevent AI detecting itself
		private Mob mob;

		private float currentRecoveryTime = 0;
		public float CurrentRecoveryTime 
		{ 
			get => currentRecoveryTime;
			set => currentRecoveryTime = value; 
		}
		private float distanceFromTarget;
		public float DistanceFromTarget
		{
			get => distanceFromTarget;
			set => distanceFromTarget = value;
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
		public LayerMask detectionLayer;
		public AIAttackAction[] aiAttacks;

		protected override void Initialize()
		{
			base.Initialize();
			Player = PlayerController.Instance.possessAtStart;
			navMeshAgent = transform.parent.GetComponentInChildren<NavMeshAgent>();
			navMeshAgent.enabled = false;
		}

		protected override void Setup()
		{
			base.Setup();
			mob = Possessed;
			AssignWeapon();
		}

		protected override void OnUpdate(float delta)
		{
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
		}

		private void AssignWeapon()
		{
			Gun weapon = weaponData.PasteItem(Containers.Instance.Items) as Gun;
			if (!weapon)
			{
				Debug.Log("Weapon assign failed");
				return;
			}
			mob.PickUpItem(weapon);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}
	}
}
