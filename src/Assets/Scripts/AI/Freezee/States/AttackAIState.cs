using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FreezeeAI
{
	public class AttackAIState : AIState
	{
		[SerializeField]
		private CombatStanceAIState combateStance;

		private AIAttackAction currentAttack;

		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Select attack base on attack score
			//if selected attack not able to be used - select new attack
			//if attack is viable, stop movement and attack target
			//set recovery timer to attacks recovery time
			//return combat stance state
			
			if (aiManager.isPerfomingAction)
			{
				return combateStance;
			}

			if (currentAttack != null)
			{
				if (aiManager.distanceFromTarget <= currentAttack.minimumDistanceNeededToAttack)
				{
					return combateStance;
				}
				else if (aiManager.distanceFromTarget <= currentAttack.maximumDistanceNeededToAttack)
				{
					if (aiManager.CurrentRecoveryTime <= 0 && aiManager.isPerfomingAction == false)
					{
						aiManager.isPerfomingAction = true;
						
						mob.UseItem(true);
						if (!mob.ActiveItem.Automatic)
							mob.UseItem(false);

						aiManager.CurrentRecoveryTime = currentAttack.recoveryTime;
						currentAttack = null;
						return combateStance;
					}
					mob.UseItem(false);
				}
			}
			return combateStance;
		}
	}
}
