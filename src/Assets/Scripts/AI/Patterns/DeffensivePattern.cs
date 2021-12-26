using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI 
{
	public class DeffensivePattern : CombatPattern
	{
		public override void Tick(AIManager aiManager, Mob mob)
		{
			
		}

		public override void AttackAction(AIManager aiManager, Mob mob)
		{
			//Нужно получать от оружия
			int minimumDistanceNeededToAttack = 1;
			int maximumDistanceNeededToAttack = 10;

			if (aiManager.distanceFromTarget <= minimumDistanceNeededToAttack)
			{
				return;
			}
			else if (aiManager.distanceFromTarget <= maximumDistanceNeededToAttack)
			{
				if (aiManager.CurrentRecoveryTime <= 0 && aiManager.isPerfomingAction == false)
				{
					aiManager.isPerfomingAction = true;

					mob.UseItem(true);
					if (!mob.ActiveItem.Automatic)
						mob.UseItem(false);

					aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
					return;
				}
				mob.UseItem(false);
			}
		}
	}
}
