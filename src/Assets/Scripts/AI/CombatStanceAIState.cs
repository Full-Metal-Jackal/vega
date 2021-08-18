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
		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			aiManager.DistanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Vector3 targetDirection = aiManager.currentTarget.transform.position - transform.position;
			Vector3 pos;

			//Check for attack range
			//potentially move around player

			//if ready to attack return attack State

			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.DistanceFromTarget <= aiManager.maxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.DistanceFromTarget + Vector3.up * mob.AimHeight;

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
				
				aiManager.nmpv.DrawPath(aiManager.navMeshAgent.path);
				Vector3 moveToPos = aiManager.navMeshAgent.desiredVelocity;
				aiManager.navMeshAgent.transform.localPosition = Vector3.zero;
				aiManager.movement = moveToPos;

				return attackState;
			}
			else if (aiManager.DistanceFromTarget > aiManager.maxAttackRange || !aiManager.CanSeeTarget)
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
	}
}
