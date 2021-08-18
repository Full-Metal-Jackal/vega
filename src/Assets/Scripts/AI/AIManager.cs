﻿using System.Collections;
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
		public NavMeshPathVisualizer nmpv;

		[HideInInspector]
		public bool isPerfomingAction;

		public bool CanSeeTarget { get; private set; }
		public Mob Player { get; private set; }
		
		private Mob mob;
		private float currentRecoveryTime = 0;
		public float currentMovementRecoveryTime = 0;

		private float distanceFromTarget;
		public float StoppingDistance { get; private set; }

		public float CurrentRecoveryTime 
		{ 
			get => currentRecoveryTime;
			set => currentRecoveryTime = value; 
		}
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
		public float maxMovementRecoveryTime = 5;
		public LayerMask detectionLayer;
		public AIAttackAction[] aiAttacks;

		protected override void Initialize()
		{
			base.Initialize();
			Player = PlayerController.Instance.possessAtStart;
			navMeshAgent = transform.parent.GetComponentInChildren<NavMeshAgent>();
			navMeshAgent.updateRotation = false;
			navMeshAgent.enabled = false;
			nmpv = transform.parent.GetComponentInChildren<NavMeshPathVisualizer>();
		}

		protected override void Setup()
		{
			base.Setup();
			mob = Possessed;
			AssignWeapon();
			StoppingDistance = maxAttackRange / 100 * 90;  //Не нашел метод который делает сам, поэтому цыфорки
		}

		protected override void OnUpdate(float delta)
		{
			CheckTargetVisibility();
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
			Gun weapon = weaponData.PasteItem(Containers.Instance.Items) as Gun;
			if (!weapon)
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

		public Vector3 FIxNavMeshPosition(Vector3 pos)
		{
			NavMeshHit hit;
			print("Pos: " + pos);
			// Check for nearest point on navmesh to agent, within onMeshThreshold
			if (NavMesh.SamplePosition(pos, out hit, 3, NavMesh.AllAreas))
			{
				// Check if the positions are vertically aligned
				if (Mathf.Approximately(pos.x, hit.position.x) && Mathf.Approximately(pos.z, hit.position.z))
				{
					print("OK");
					return pos;
				}
			}
			if (NavMesh.FindClosestEdge(pos, out hit, NavMesh.AllAreas))
			{
				print("HIT: " + hit.position);
				return hit.position;
			}
			else
			{
				return Player.transform.position;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}
	}
}
