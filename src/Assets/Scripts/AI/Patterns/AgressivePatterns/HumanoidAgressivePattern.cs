using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class HumanoidAgressivePattern : AgressivePattern
	{
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			Vector3 targetDirection = aiManager.currentTarget.transform.position - aiManager.transform.position;
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.maxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * (mob.AimHeight - 1f);

				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					if (RandomMovementPos(aiManager, targetDirection, out Vector3 newPos))
					{
						aiManager.NavMeshAgent.enabled = true;
						aiManager.NavMeshObstacle.enabled = false;
						pos = newPos;
						aiManager.NavMeshAgent.SetDestination(pos);
						aiManager.currentMovementRecoveryTime = aiManager.maxMovementRecoveryTime;
					}
				}

				MoveToLastPos(aiManager);
				AttackAction(aiManager, mob);
			}
		}

		public override void AttackAction(AIManager aiManager, Mob mob)
		{
			//Нужно получать от оружия но пока и так сойдет
			int minimumDistanceNeededToAttack = 1;
			int maximumDistanceNeededToAttack = 10;
			float recoveryTime = 0.4f;


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

					aiManager.CurrentRecoveryTime = recoveryTime;
					return;
				}
				mob.UseItem(false);
			}
		}
	}
}
