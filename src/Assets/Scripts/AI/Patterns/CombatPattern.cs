using System;
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

		private float curentAngle = 0;

		public abstract void Tick(AIManager aiManager, Mob mob);

		public abstract void AttackAction(AIManager aiManager, Mob mob);
		
		protected bool RandomMovementPos(AIManager aiManager, Vector3 targetDirection, out Vector3 point)
		{
			NavMeshHit hit;
			Vector3 pointInSphere = UnityEngine.Random.insideUnitSphere * aiManager.MaxAttackRange;
			pointInSphere.y = 0;

			Vector3 randomPoint = aiManager.currentTarget.transform.position + pointInSphere;

			Vector3 newPosDir = (randomPoint - transform.position).normalized;
			float angle = Vector3.SignedAngle(newPosDir, targetDirection.normalized, Vector3.up);

			if (aiManager.distanceFromTarget < aiManager.DangerThreshhold && Mathf.Abs(angle) < closeAvoidanceAngle) //Checking that the new point is not in the target's direction
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
			Vector3 pointInSphere = UnityEngine.Random.insideUnitSphere * cover.radius;
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
			Vector3 moveToPos = aiManager.NavMeshAgent.desiredVelocity;
			aiManager.NavMeshAgent.transform.localPosition = Vector3.zero;
			aiManager.movement = moveToPos;
		}

		protected bool MoveAroundTarget(AIManager aiManager, out Vector3 point)
		{
			NavMeshHit hit;
			float step = 20f;
			float radius = aiManager.MaxAttackRange * 0.5f;
			float centerX = aiManager.currentTarget.transform.position.x;
			float centerZ = aiManager.currentTarget.transform.position.z;
			curentAngle += step;
			double angle = Math.PI * curentAngle / 180.0;
			float pointX = centerX + radius * (float) Math.Cos(angle);
			float pointZ = centerZ + radius * (float) Math.Sin(angle);
			Vector3 pointOnCircle = new Vector3(pointX, 0.1f, pointZ);

			if (NavMesh.SamplePosition(pointOnCircle, out hit, 0.5f, NavMesh.AllAreas))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		protected void DashInRandomDirection(AIManager aiManager, Mob mob)
		{
			NavMeshHit hit;
			Vector3 pointInSphere = UnityEngine.Random.insideUnitSphere;
			pointInSphere.y = 0;

			Vector3 randomPoint = mob.transform.position + pointInSphere;

			Vector3 dodgesDir = (randomPoint - transform.position).normalized;
			
			if (mob.CanMoveActively)
			{
				mob.DashAction();
			}
		}
	}
}
