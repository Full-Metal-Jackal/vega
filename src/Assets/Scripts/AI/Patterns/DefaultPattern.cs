using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class DefaultPattern : CombatPattern
	{
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			Vector3 targetDirection = aiManager.currentTarget.transform.position - aiManager.transform.position;
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.MaxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;

				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					if (RandomMovementPos(aiManager, targetDirection, out Vector3 newPos))
					{
						aiManager.NavMeshAgent.enabled = true;
						aiManager.NavMeshObstacle.enabled = false;
						pos = newPos;
						aiManager.NavMeshAgent.SetDestination(pos);
						aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
					}
				}

				MoveToLastPos(aiManager);
				AttackAction(aiManager, mob);
			}
		}

		public override void AttackAction(AIManager aiManager, Mob mob)
		{
			//Нужно получать от оружия
			int minimumDistanceNeededToAttack = 1;
			int maximumDistanceNeededToAttack = 10;

			if (aiManager.distanceFromTarget <= maximumDistanceNeededToAttack && aiManager.distanceFromTarget >= minimumDistanceNeededToAttack)
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
