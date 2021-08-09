using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class AIManager : MobController
	{
		AILocomotionManager aiLocomotionManager;
		public bool isPerfomingAction = true;

		[Header("A.I Settings")]
		public float detectionRadius = 5;
		public float maxDetectionAngle = 50;
		public float minDetectionAngle = -50;

		protected override void Initialize()
		{
			base.Initialize();
		    aiLocomotionManager = GetComponent<AILocomotionManager>();
	}
		protected override void OnUpdate(float delta)
		{
			HandleCurrentAction(delta);
		}

		private void HandleCurrentAction(float delta)
		{
			if (aiLocomotionManager.currentTarget == null)
			{
				aiLocomotionManager.HandleDetection();
			}
			else if (aiLocomotionManager.distanceFromTarget > aiLocomotionManager.stoppingDistance)
			{
				aiLocomotionManager.HandleMoveToTarget(delta);
			}
			else if (aiLocomotionManager.distanceFromTarget <= aiLocomotionManager.stoppingDistance)
			{
				//Handle Attack
			}
		}

		#region Attacks
		private void GetNewAttack()
		{
			Vector3 targetDirection = aiLocomotionManager.currentTarget.transform.position - transform.position;
			float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
		}
		#endregion

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}
	}
}

