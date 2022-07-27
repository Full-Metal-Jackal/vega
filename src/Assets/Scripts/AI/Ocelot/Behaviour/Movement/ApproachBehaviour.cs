using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OcelotAI
{
	public class ApproachBehaviour : AIBehaviour
	{
		/// <summary>
		/// Distance to the target the mob should establish.
		/// </summary>
		[SerializeField]
		private float distance = 7.5f;

		[SerializeField]
		private bool stopOnApproach = false;

		protected override void UpdateControl(AIController controller)
		{
			Vector3 targetPos = controller.Target.transform.position;
			
			Complete = Vector3.Distance(
				targetPos, controller.Possessed.transform.position
			) <= distance;

			if (!stopOnApproach || !Complete)
				MoveTo(targetPos);

			Aim(
				controller.Possessed.AimOrigin +
				controller.Possessed.transform.forward
			);
		}
	}
}
