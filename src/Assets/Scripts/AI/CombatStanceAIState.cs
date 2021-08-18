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

		private const int aimRange = 5;
		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			aiManager.DistanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Vector3 targetDirection = aiManager.currentTarget.transform.position - transform.position;
			Vector3 pos = mob.transform.position;
			//aiManager.movement = Vector3.zero;

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
						aiManager.currentMovementRecoveryTime = 3;
						aiManager.navMeshAgent.enabled = true;
						pos = newPos;
					}
				}
				aiManager.navMeshAgent.SetDestination(pos);
				aiManager.navMeshAgent.transform.localPosition = Vector3.zero;
				aiManager.movement = aiManager.navMeshAgent.desiredVelocity;
				Debug.DrawRay(pos, pos + Vector3.up * 10, Color.blue);
				return attackState;
			}
			else if (aiManager.DistanceFromTarget > aiManager.maxAttackRange || !aiManager.CanSeeTarget)
			{
				return chaseState;
			}
			/*
			else if (aiManager.DistanceFromTarget < aiManager.maxAttackRange / 2)  //временая константа
			{
				print("FLEE");
				Flee(aiManager, targetDirection);
				return attackState;
			}*/
			else
			{
				return this;
			}
		}

		/*
		private void Flee(AIManager aiManager, Vector3 targetDirection)
		{
			float delta = Time.deltaTime;
			Vector3 potentialPos = transform.position - targetDirection.normalized * 2;  //TODO Check newPos for availability
			aiManager.navMeshAgent.enabled = true;
			print("POtPos: " + potentialPos);
			Vector3 newPos = aiManager.FIxNavMeshPosition(potentialPos);
			print("NewPos: " + newPos);
			aiManager.navMeshAgent.SetDestination(newPos);
			transform.parent.rotation = Quaternion.Slerp(transform.rotation,
														 aiManager.navMeshAgent.transform.rotation,
														 aiManager.rotationSpeed / delta);
			aiManager.movement = aiManager.navMeshAgent.desiredVelocity;
		}
		*/
		private bool RandomMovementPos(AIManager aiManager, Vector3 targetDirection, out Vector3 point)
		{
			NavMeshHit hit;
			Vector3 targetPos = aiManager.currentTarget.transform.position;
			Vector3 pp = Random.insideUnitSphere * aiManager.maxAttackRange;
			pp.y = 0;
			Vector3 randomPoint = transform.position + pp;

			Vector3 newPosDir = (randomPoint - transform.position).normalized;
			float angle = Vector3.SignedAngle(newPosDir, targetDirection.normalized, Vector3.up);
			if (Mathf.Abs(angle) < 90)
			{
				print("Bad ponint: " + angle);
				point = Vector3.zero;
				return false;
			}
			else
			{
				if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
				{
					print("Good point: " + angle);
					point = hit.position;
					return true;
				}
			}
			/*
			float dist1 = Vector3.Distance(targetPos, randomPoint);
			//print(dist1);
			float gipotenuz = Mathf.Sqrt(2 * aiManager.DistanceFromTarget * aiManager.DistanceFromTarget);
			if (dist1 < gipotenuz)
			{
				print("Bad ponint");
				point = Vector3.zero;
				return false;
			}
			*/
			print("mem");
			point = Vector3.zero;
			return false;
		}
	}
}
