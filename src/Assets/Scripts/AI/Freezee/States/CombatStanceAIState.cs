using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FreezeeAI
{
	public class CombatStanceAIState : AIState
	{
		[SerializeField]
		private ChaseAIState chaseState;
		[SerializeField]
		private IdleAIState idleState;
		[SerializeField]
		private PatternSelector patternSelector;

		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Сбор информации об окружении на текущий тик
			if (aiManager.PatternRecoveryTime <= 0)
			{
				aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

				//Выбрать паттерн исходя их собранной информации

				CombatPattern pattern = patternSelector.SelectPattern(aiManager);
				aiManager.PatternRecoveryTime = pattern.duration;

				//Выполнить паттерн

				aiManager.currentPattern = pattern;
			}

			if (aiManager.distanceFromTarget > aiManager.MaxAttackRange || !aiManager.CanSeeTarget)
			{
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				return chaseState;
			}
			return this;
		}
	}
}
