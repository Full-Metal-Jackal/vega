using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class AIManager : MobController
	{
		AILocomotionManager aiLocomotionManager;
		bool isPerfomingAction;

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
			HandleCurrentAction();
		}

		private void HandleCurrentAction()
		{
			if (aiLocomotionManager.currentTarget == null)
			{
				aiLocomotionManager.HandleDetection();
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red; //replace red with whatever color you prefer
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}
	}
}

