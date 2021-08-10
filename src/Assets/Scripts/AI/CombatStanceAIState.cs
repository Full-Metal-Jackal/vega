using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class CombatStanceAIState : AIState
	{
		public AttackAIState attackState;
		public ChaseAIState chaseState;
		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			aiManager.DistanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Vector3 targetDirection = aiManager.currentTarget.transform.position - transform.position;
			//Check for attack range
			//potentially move around player


			//if ready to attack return attack State
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.DistanceFromTarget <= aiManager.maxAttackRange)
			{
				return attackState;
			}
			else if (aiManager.DistanceFromTarget > aiManager.maxAttackRange)
			{
				return chaseState;
			}
			else
			{
				return this;
			}
		}
	}
}

