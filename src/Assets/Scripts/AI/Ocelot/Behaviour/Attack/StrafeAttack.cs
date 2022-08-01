using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OcelotAI
{
	public class StrafeAttack : SimpleAttack
	{
		private int strafeSide = 1;

		protected override void ActivateControl(AIController controller)
		{
			base.ActivateControl(controller);
			
			strafeSide = -strafeSide;
		}

		protected override void UpdateControl(AIController controller)
		{
			Aim(controller.InterceptionShotPosition(controller.Target));
			SetTrigger(true);

			Vector3 direction = controller.Target.transform.position -
				controller.Possessed.transform.position;

			direction = Quaternion.AngleAxis(
				90, controller.Target.transform.up * strafeSide
			) * direction;

			MoveDirect(direction);
		}
	}
}
