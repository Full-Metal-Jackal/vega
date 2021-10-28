using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AI
{
	public abstract class CombatPattern : MonoBehaviour
	{
		public float duration;  //Колличество Тиков патерна
		protected bool isAvailable;

		protected const int normalAvoidanceAngle = 45;
		protected const int closeAvoidanceAngle = 90;

		public abstract void Tick(AIManager aiManager, Mob mob);

		protected bool RandomMovementPos(AIManager aiManager, Vector3 targetDirection, out Vector3 point)
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

		protected bool FixCoverPos(CoverSpot cover, out Vector3 point)
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

		protected void MoveToLastPos(AIManager aiManager)
		{
			aiManager.NavMeshVisualizer.DrawPath(aiManager.NavMeshAgent.path);
			Vector3 moveToPos = aiManager.NavMeshAgent.desiredVelocity;
			aiManager.NavMeshAgent.transform.localPosition = Vector3.zero;
			aiManager.movement = moveToPos;
		}
	}
}
