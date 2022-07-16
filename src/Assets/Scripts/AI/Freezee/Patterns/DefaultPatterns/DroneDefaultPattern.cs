using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FreezeeAI
{
	public class DroneDefaultPattern : DefaultPattern
	{
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.MaxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = aiManager.TargetPos;

				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					if (RandomMovementPos(aiManager, out Vector3 newPos))
					{
						aiManager.NavMeshAgent.enabled = true;
						aiManager.NavMeshObstacle.enabled = false;
						pos = newPos;
						aiManager.NavMeshAgent.SetDestination(pos);
						aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
					}
				}

				MoveToLastPos(aiManager);
				// waitForMovement
				AttackAction(aiManager, mob);
				// waitForAttack
			}
		}

		public override void AttackAction(AIManager aiManager, Mob mob)
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

