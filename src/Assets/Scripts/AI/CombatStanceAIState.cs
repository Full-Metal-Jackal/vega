using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
	public class CombatStanceAIState : AIState
	{
		[SerializeField]
		private AttackAIState attackState;
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
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Dictionary<string, float> data = CollectData(aiManager);

			//Выбрать паттерн исходя их собранной информации

			CombatPattern pattern = SelectPattern(aiManager, data);

			//Выполнить паттерн

			float timer = 0;
			while(timer < pattern.duration)
			{
				pattern.Tick(aiManager, mob);
				timer++;
			}

			//Повторить


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
			return chaseState;
		}

		/*
		private bool RandomMovementPos(AIManager aiManager, Vector3 targetDirection, out Vector3 point)
		{
			NavMeshHit hit;
			Vector3 pointInSphere = Random.insideUnitSphere * aiManager.maxAttackRange;
			pointInSphere.y = 0;
			Vector3 randomPoint = transform.position + pointInSphere;

			Vector3 newPosDir = (randomPoint - transform.position).normalized;
			float angle = Vector3.SignedAngle(newPosDir, targetDirection.normalized, Vector3.up);

			if (aiManager.distanceFromTarget < aiManager.dangerThreshhold && Mathf.Abs(angle) < closeAvoidanceAngle) //Checking that the new point is not in the target's direction
			{
				point = Vector3.zero;
				return false;
			}
			else if (Mathf.Abs(angle) < normalAvoidanceAngle)
			{
				point = Vector3.zero;
				return false;
			}
			else if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		private bool FixCoverPos(CoverSpot cover, out Vector3 point)
		{
			NavMeshHit hit;
			Vector3 pointInSphere = Random.insideUnitSphere * cover.radius;
			Vector3 randomPoint = cover.transform.position + pointInSphere;
			Vector3 randP = randomPoint;
			randP.y = 0;
			if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, -1 << NavMesh.GetAreaFromName("Cover")))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		private void MoveToLastPos(AIManager aiManager)
		{
			aiManager.NavMeshVisualizer.DrawPath(aiManager.NavMeshAgent.path);
			Vector3 moveToPos = aiManager.NavMeshAgent.desiredVelocity;
			aiManager.NavMeshAgent.transform.localPosition = Vector3.zero;
			aiManager.movement = moveToPos;
		}
		*/

		private CombatPattern SelectPattern(AIManager aiManager, Dictionary<string, float> data)
		{
			CombatPattern pattern = null;

			/*
			 * MinMaxScaller сюда воткнуть
			 */

			//Вычисляется status моба по формуле
			float status = 0.5f;

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
			
	}
}
