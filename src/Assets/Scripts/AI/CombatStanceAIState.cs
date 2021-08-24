using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
	public class CombatStanceAIState : AIState
	{
		public AttackAIState attackState;
		public ChaseAIState chaseState;
		public IdleAIState idleState;
		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Vector3 targetDirection = aiManager.currentTarget.transform.position - transform.position;
			Vector3 pos;
			CoverSpot newCover;

			//Check for attack range
			//prioritize moving to cover

			//if ready to attack return attack State

			if (aiManager.currentCover != null)
			{
				float dist = Vector3.Distance(transform.position, aiManager.currentCover.transform.position);
				if (!aiManager.InCover)
				{
					MoveToLastPos(aiManager);
					return this;
				}
				else if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.maxAttackRange && aiManager.CanSeeTarget)
				{
					mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;

					return attackState;
				}
				else if (dist < 1.0f)
				{
					aiManager.movement = Vector3.zero;
				}
				return this;
			}
			else if (aiManager.FindCover(out newCover))
			{   
				if (FixCoverPos(newCover, out Vector3 newPos))
				{
					aiManager.navMeshAgent.enabled = true;
					print("Found Cover with position in: " + newPos);
					pos = newPos;
					aiManager.navMeshAgent.SetDestination(pos);
					aiManager.currentCover = newCover;
				}
				return this;
			}
			else if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.maxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
				
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					if (RandomMovementPos(aiManager, targetDirection, out Vector3 newPos))
					{		
						aiManager.navMeshAgent.enabled = true;
						pos = newPos;
						aiManager.navMeshAgent.SetDestination(pos);
						aiManager.currentMovementRecoveryTime = aiManager.maxMovementRecoveryTime;
					}
				}

				MoveToLastPos(aiManager);

				return attackState;
			}
			else if (aiManager.distanceFromTarget > aiManager.maxAttackRange || !aiManager.CanSeeTarget)
			{
				aiManager.navMeshAgent.enabled = false;
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
			if (Mathf.Abs(angle) < 90)  //Checking that the new point is not in the target's direction
			{
				point = Vector3.zero;
				return false;
			}
			else
			{
				if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
				{
					point = hit.position;
					return true;
				}
			}
			point = Vector3.zero;
			return false;
		}

		private bool FixCoverPos(CoverSpot cover, out Vector3 point)
		{
			NavMeshHit hit;
			Vector3 pointInSphere = Random.insideUnitSphere * cover.radius;
			//pointInSphere.y = 0;
			Vector3 randomPoint = cover.transform.position + pointInSphere;
			print("RandomPoint: " + randomPoint);
			Vector3 randP = randomPoint;
			randP.y = 0;
			if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, -1 << 18))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		private void MoveToLastPos(AIManager aiManager)
		{
			aiManager.navMeshVisualizer.DrawPath(aiManager.navMeshAgent.path);
			Vector3 moveToPos = aiManager.navMeshAgent.desiredVelocity;
			aiManager.navMeshAgent.transform.localPosition = Vector3.zero;
			aiManager.movement = moveToPos;
		}
	}
}
