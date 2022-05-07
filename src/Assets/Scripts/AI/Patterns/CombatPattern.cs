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
		
		protected bool RandomMovementPos(AIManager aiManager, out Vector3 point)
		{
			Vector3 pointInSphere = UnityEngine.Random.insideUnitSphere * aiManager.MaxAttackRange;
			pointInSphere.y = 0;

			Vector3 randomPoint = aiManager.currentTarget.transform.position + pointInSphere;

			Vector3 newPosDir = (randomPoint - transform.position).normalized;
			float angle = Vector3.SignedAngle(newPosDir, aiManager.DefaultTargetDirection.normalized, Vector3.up);

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
			else if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		protected bool FixCoverPos(CoverSpot cover, out Vector3 point)
		{
			Vector3 pointInSphere = UnityEngine.Random.insideUnitSphere * cover.radius;
			Vector3 randomPoint = cover.transform.position + pointInSphere;
			Vector3 randP = randomPoint;
			randP.y = 0;
			if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 0.1f, -1 << NavMesh.GetAreaFromName("Cover")))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		protected bool FixPos(Vector3 pos, out Vector3 point)
		{
			if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}

		protected void MoveToLastPos(AIManager aiManager)
		{
			aiManager.NavMeshAgent.transform.localPosition = Vector3.zero;
			Vector3 moveToPos = aiManager.NavMeshAgent.desiredVelocity;
			aiManager.movement = moveToPos;
		}

		protected bool MoveAroundTarget(AIManager aiManager, out Vector3 point)
		{
			float step = 20f;
			float radius = aiManager.MaxAttackRange * 0.5f;
			float centerX = aiManager.currentTarget.transform.position.x;
			float centerZ = aiManager.currentTarget.transform.position.z;
			curentAngle += step;
			double angle = Math.PI * curentAngle / 180.0;
			float pointX = centerX + radius * (float) Math.Cos(angle);
			float pointZ = centerZ + radius * (float) Math.Sin(angle);
			Vector3 pointOnCircle = new Vector3(pointX, 0.1f, pointZ);

			if (NavMesh.SamplePosition(pointOnCircle, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
			{
				point = hit.position;
				return true;
			}
			point = Vector3.zero;
			return false;
		}
		
		protected Vector3 RotatePointOnAngle(Vector3 pointToRotate, Vector3 centerPoint, float angleInDegrees, int dir)
		{
			double angleInRadians = Math.PI * angleInDegrees / 180.0 * dir;
			float cosTheta = (float) Math.Cos(angleInRadians);
			float sinTheta = (float) Math.Sin(angleInRadians);

			float X = cosTheta * (pointToRotate.x - centerPoint.x) - sinTheta * (pointToRotate.z - centerPoint.z) + centerPoint.x;
			float Z = sinTheta * (pointToRotate.x - centerPoint.x) + cosTheta * (pointToRotate.z - centerPoint.z) + centerPoint.z;
			return new Vector3(X, 0, Z);
		}

		protected Vector3 MovePointAroundCenter(Vector3 center, Vector3 curentPos)
		{
			float step = 5f;
			float offsetX = center.x - curentPos.x;
			float offsetZ = center.z - curentPos.z;
			double angle = Math.PI * step / 180.0;
			float pointX = offsetX * (float)Math.Cos(angle) - offsetZ * (float)Math.Sin(angle) + center.x;
			float pointZ = offsetX * (float)Math.Sin(angle) + offsetZ * (float)Math.Cos(angle) + center.z;

			return new Vector3(pointX, curentPos.y, pointZ);
		}

		protected Vector3 AimWithPrediction(AIManager aiManager, Mob mob)
		{
			float projectileSpeed = (mob.ActiveItem is Gun gun) ? gun.ProjectileSpeed : 15f;

			Vector3 targetVelocity = aiManager.currentTarget.GetComponent<Rigidbody>().velocity;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			Vector3 aimPos = mob.transform.position + aiManager.DefaultTargetDirection.normalized * aiManager.distanceFromTarget +
				targetVelocity * aiManager.distanceFromTarget / projectileSpeed + Vector3.up * aiManager.currentTarget.AimHeight;

			return aimPos;
		}
	}
}
