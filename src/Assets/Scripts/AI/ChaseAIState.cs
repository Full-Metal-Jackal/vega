using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class ChaseAIState : AIState
	{
		public CombatStanceAIState combatStanceState;
		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Chase target
			//if within attack range, switch to combat state
			//else return this
			if (aiManager.isPerfomingAction)
			{
				return this;
			}
			float delta = Time.deltaTime;
			Vector3 targetDirection = GetNavMeshDirection(delta, aiManager);
			float distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, transform.position);

			if (distanceFromTarget > aiManager.maxAttackRange)
			{
				aiManager.movement = targetDirection;
			}

			aiManager.navMeshAgent.transform.localPosition = Vector3.zero;
			aiManager.navMeshAgent.transform.localRotation = Quaternion.identity;

			if (distanceFromTarget <= aiManager.maxAttackRange)
			{
				return combatStanceState;
			}
			else
			{
				return this;
			}
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
				aiManager.navMeshAgent.enabled = true;
				aiManager.navMeshAgent.SetDestination(aiManager.currentTarget.transform.position);
				transform.parent.rotation = Quaternion.Slerp(transform.rotation, 
															 aiManager.navMeshAgent.transform.rotation, 
															 aiManager.rotationSpeed / delta);
				targetDirection = aiManager.navMeshAgent.desiredVelocity;
			}

			return targetDirection;
		}
	}
}

