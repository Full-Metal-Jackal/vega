using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OcelotAI
{
	public class SimpleAttack : AIBehaviour
	{
		[SerializeField]
		private bool overrideMovement = true;

		[SerializeField]
		private int maxShots = 0;
		protected int remainingShots = 0;

		protected override void UpdateControl(AIController controller)
		{
			if (overrideMovement)
				MoveDirect(Vector3.zero);

			Aim(controller.InterceptionShotPosition(controller.Target));
			
			if (maxShots <= 0 || remainingShots > 0)
				SetTrigger(true);
		}

		protected void CountShot()
		{
			if (--remainingShots <= 0)
				Complete = true;
		}

		protected override void ActivateControl(AIController controller)
		{
			// <TODO> Unoptimized because of reassigning every activation, needs refactoring
			if (maxShots > 0)
			{
				if (controller.Gun)
					controller.Gun.OnAfterFire += CountShot;
				remainingShots = maxShots;
			}
		}

		protected override void DeactivateControl(AIController controller)
		{
			if (maxShots > 0 && controller.Gun)
				controller.Gun.OnAfterFire -= CountShot;

			SetTrigger(false);
		}
	}
}
