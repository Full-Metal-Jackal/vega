using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
	public class CombatStanceAIState : AIState
	{
		[SerializeField]
		private ChaseAIState chaseState;
		[SerializeField]
		private IdleAIState idleState;
		[SerializeField]
		private DefaultPattern defaultPattern;
		[SerializeField]
		private AgressivePattern agressivePattern;
		[SerializeField]
		private DeffensivePattern deffensivePattern;

		private const int normalAvoidanceAngle = 45;
		private const int closeAvoidanceAngle = 90;
		private const float agressiveTreshhold = 0.75f;
		private const float deffensiveTreshhold = 0.25f;

		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Сбор информации об окружении на текущий тик
			if (aiManager.PatternRecoveryTime <= 0)
			{
				aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
				Dictionary<string, float> data = CollectData(aiManager);

				//Выбрать паттерн исходя их собранной информации

				CombatPattern pattern = SelectPattern(aiManager, data);
				aiManager.PatternRecoveryTime = pattern.duration;

				//Выполнить паттерн

				aiManager.currentPattern = pattern;
			}
			
			//Повторить

			if (aiManager.distanceFromTarget > aiManager.maxAttackRange || !aiManager.CanSeeTarget)
			{
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				return chaseState;
			}
			else
			{
				return this;
			}

			/*

			//Check for attack range
			//prioritize moving to cover

			//if ready to attack return attack State
			if (aiManager.currentTarget == null)
			{
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				return idleState;
			}

			if (aiManager.currentCover != null)
			{
				if (!aiManager.IsCurrentCoverRelevant())
				{
					return this;
				}
				float dist = Vector3.Distance(transform.position, aiManager.currentCover.transform.position);
				if (!aiManager.inCover)
				{
					MoveToLastPos(aiManager);
					return this;
				}
				else if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.maxAttackRange && aiManager.CanSeeTarget)
				{
					mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;

					return attackState;
				}
				if (dist < 1.0f)
				{
					aiManager.movement = Vector3.zero;
				}
				return this;
			}
			else if (aiManager.FindCover(out CoverSpot newCover))
			{   
				if (FixCoverPos(newCover, out Vector3 newPos))
				{
					aiManager.NavMeshAgent.enabled = true;
					aiManager.NavMeshObstacle.enabled = false;
					pos = newPos;
					aiManager.NavMeshAgent.SetDestination(pos);
					aiManager.currentCover = newCover;
				}
				return this;
			}
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.maxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
				
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					if (RandomMovementPos(aiManager, targetDirection, out Vector3 newPos))
					{
						aiManager.NavMeshAgent.enabled = true;
						aiManager.NavMeshObstacle.enabled = false;
						pos = newPos;
						aiManager.NavMeshAgent.SetDestination(pos);
						aiManager.currentMovementRecoveryTime = aiManager.maxMovementRecoveryTime;
					}
				}

				MoveToLastPos(aiManager);
				
				return attackState;
			}
			else if (aiManager.distanceFromTarget > aiManager.maxAttackRange || !aiManager.CanSeeTarget)
			{
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				return chaseState;
			}
			else
			{
				return this;
			}
			*/

		}

		private CombatPattern SelectPattern(AIManager aiManager, Dictionary<string, float> data)
		{
			CombatPattern pattern = null;

			/*
			 * MinMaxScaller сюда воткнуть
			 */

			//Вычисляется status моба по формуле
			float status = 0.5f;
			print($"{aiManager.Possessed} switched to pattern {pattern}");
			if (status <= deffensiveTreshhold)
			{
				return deffensivePattern;
			}
			else if (status >= agressiveTreshhold)
			{
				return agressivePattern;
			}
			else 
			{
				return defaultPattern;
			}
		}

		private Dictionary<string, float> CollectData(AIManager aiManager)
		{
			//aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			float distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			float mobHp = aiManager.Possessed.Health;
			float targetHp = aiManager.currentTarget.Health;
			float targetKS = 1;

			Dictionary<string, float> data = new Dictionary<string, float>();
			data.Add("mobHp", mobHp);
			data.Add("targetHp", targetHp);
			data.Add("targetKS", targetKS);
			data.Add("DistanceFromTarget", distanceFromTarget);

			return data;

		}

		private void AttackAction(AIManager aiManager, Mob mob)
		{
			//Нужно получать от оружия
			int minimumDistanceNeededToAttack = 1;
			int maximumDistanceNeededToAttack = 10;
			float recoveryTime = 0.2f;


			if (aiManager.distanceFromTarget <= minimumDistanceNeededToAttack)
			{
				return;
			}
			else if (aiManager.distanceFromTarget <= maximumDistanceNeededToAttack)
			{
				if (aiManager.CurrentRecoveryTime <= 0 && aiManager.isPerfomingAction == false)
				{
					aiManager.isPerfomingAction = true;

					mob.UseItem(true);
					if (!mob.ActiveItem.Automatic)
						mob.UseItem(false);

					aiManager.CurrentRecoveryTime = recoveryTime;
					return;
				}
				mob.UseItem(false);
			}
		}	
	}
}
