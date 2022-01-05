using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
	public class ChaseAIState : AIState
	{
		[SerializeField]
		private CombatStanceAIState combatStanceState;
		[SerializeField]
		private IdleAIState idleState;

		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Chase target
			//if within attack range, switch to combat state
			//else return this
			if (aiManager.isPerfomingAction)
			{
				return this;
			}
			if (aiManager.currentTarget == null)
			{
				return idleState;
			}
			float delta = Time.deltaTime;
			Vector3 targetDirection = GetNavMeshDirection(delta, aiManager);
			float distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, transform.position);

			//NavMeshPath path = aiManager.NavMeshAgent.path;
			//aiManager.NavMeshVisualizer.DrawPath(path);

			aiManager.NavMeshAgent.transform.localPosition = Vector3.zero;

			if (distanceFromTarget <= aiManager.StoppingDistance && aiManager.CanSeeTarget)
			{
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				aiManager.movement = Vector3.zero;
				return combatStanceState;
			}

			aiManager.movement = targetDirection;
			mob.AimPos = mob.transform.position + targetDirection.normalized * distanceFromTarget;
			return this;

		}

		public Vector3 GetNavMeshDirection(float delta, AIManager aiManager)
		{
			Vector3 targetDirection;
			//Move manualy
			if (aiManager.isPerfomingAction)
			{
				targetDirection = aiManager.currentTarget.transform.position - transform.position;
			}
			// Move via pathfinding	
			else
			{
				aiManager.NavMeshAgent.enabled = true;
				aiManager.NavMeshObstacle.enabled = false;
				aiManager.NavMeshAgent.SetDestination(aiManager.currentTarget.transform.position);

				targetDirection = aiManager.NavMeshAgent.desiredVelocity;
			}

			return targetDirection;
		}
	}
}


