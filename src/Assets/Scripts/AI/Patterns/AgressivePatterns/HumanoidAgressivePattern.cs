﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class HumanoidAgressivePattern : AgressivePattern
	{
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.MaxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = mob.transform.position + aiManager.DefaultTargetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * (mob.AimHeight - 1f);

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
				AttackAction(aiManager, mob);
			}
		}
	}
}
