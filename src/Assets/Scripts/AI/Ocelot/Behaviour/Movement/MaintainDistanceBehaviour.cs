using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OcelotAI
{
	public class MaintainDistanceBehaviour : AIBehaviour
	{
		/// <summary>
		/// Distance to the target the mob should establish.
		/// </summary>
		[SerializeField]
		private float distance = 7.5f;

		/// <summary>
		/// Distance error allowed to assume that the target deistance is being maintained.
		/// </summary>
		[SerializeField]
		private float distanceTolerance = .5f;

		/// <summary>
		/// If true, the mob will circle around the target as long as the behaviour is active.
		/// </summary>
		[SerializeField]
		private bool neverStop = false;

		private int strafeSide = 1;

		protected override void UpdateControl(AIController controller)
		{
			Vector3 targetPos = controller.Target.transform.position;
			Vector3 mobPos = controller.Possessed.transform.position;

			Vector3 difference = targetPos - mobPos;

			float distanceToRing = difference.magnitude - distance;

			Complete = Mathf.Abs(distanceToRing) < distanceTolerance;

			if (neverStop || !Complete)
			{
				Vector3 destination = mobPos + difference.normalized * distanceToRing;

				if (neverStop)
				{
					// To prevent jittering, the mob starts to strafe as it approaches the target distance.
					float strafeAmount = (1f - Mathf.Clamp01(Mathf.Abs(distanceToRing) / distance));
					Quaternion strafeRotation = Quaternion.AngleAxis(
						strafeAmount * controller.Possessed.MoveSpeed,
						controller.Target.transform.up * strafeSide
					);

					destination = targetPos + (strafeRotation * (destination - targetPos));
				}

				MoveTo(destination);
			}
			else
			{
				MoveDirect(Vector3.zero);
			}

			Aim(
				controller.Possessed.AimOrigin +
				controller.Possessed.transform.forward
			);
		}

		protected override void DeactivateControl(AIController controller)
		{
			base.DeactivateControl(controller);

			strafeSide = -strafeSide;
		}
	}
}
