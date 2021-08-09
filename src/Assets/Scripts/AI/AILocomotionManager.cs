using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AI
{
	public class AILocomotionManager : MonoBehaviour
	{

		AIManager aiManager;
		MobAnimationHandler aiAnimatorHandler;
		Rigidbody aiRigidBody;
		public NavMeshAgent navMeshAgent;
		public Mob currentTarget;
		public LayerMask detectionLayer;
		public Mob player;  //needs to prevent AI detecting itself
		private Mob mob;
		private MobController controller;
		

		public float distanceFromTarget;
		public float stoppingDistance = 0.5f;

		public float rotationSpeed = 15;

		private void Awake()
		{
			aiManager = GetComponent<AIManager>();
			aiAnimatorHandler = transform.parent.GetComponentInChildren<MobAnimationHandler>();
			navMeshAgent = transform.parent.GetComponentInChildren<NavMeshAgent>();
			mob = transform.parent.GetComponent<Mob>();
			aiRigidBody = transform.parent.GetComponent<Rigidbody>();
		}
		public void HandleDetection()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, aiManager.detectionRadius, detectionLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				Mob character = colliders[i].transform.parent.GetComponent<Mob>();

				if (character != null)
				{
					/* TODO
					 *  Check for team. If AI can attack each other.
					 */
					if (character == player)
					{
						Vector3 targetDirection = character.transform.position - transform.position;
						float viewAngle = Vector3.Angle(targetDirection, transform.forward);

						if (viewAngle > aiManager.minDetectionAngle && viewAngle < aiManager.maxDetectionAngle)
						{
							currentTarget = character;
						}
					}
				}
			}
		}

		public void HandleMoveToTarget(float delta)
		{
			Vector3 targetDirection = GetNavMeshDirection(delta);
			distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
			float viewableAngle = Vector3.Angle(targetDirection, transform.forward);


			if (aiManager.isPerfomingAction)
			{
				aiManager.movement = Vector3.zero;
				navMeshAgent.enabled = false;
			}
			else
			{
				if (distanceFromTarget > stoppingDistance)
				{
					aiManager.movement = targetDirection;
				}
				else
				{
					aiManager.movement = Vector3.zero;
				}
			}

			navMeshAgent.transform.localPosition = Vector3.zero;
			navMeshAgent.transform.localRotation = Quaternion.identity;
		}

		public Vector3 GetNavMeshDirection(float delta)
		{
			Vector3 targetDirection;
			//Move manualy
			if (aiManager.isPerfomingAction)
			{
				targetDirection = currentTarget.transform.position - transform.position;
				
			}
			// Move via pathfinding	
			else
			{
				navMeshAgent.enabled = true;
				navMeshAgent.SetDestination(currentTarget.transform.position);
				transform.parent.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / delta);
				targetDirection = navMeshAgent.desiredVelocity;
			}

			return targetDirection;
		}
	}
}

