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

		private const int normalAvoidanceAngle = 45;
		private const int closeAvoidanceAngle = 90;

		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Vector3 targetDirection = aiManager.currentTarget.transform.position - transform.position;
			Vector3 pos;

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
		}

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
	}
}
