﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class DroneAgressivePattern : AgressivePattern
	{
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= aiManager.MaxAttackRange && aiManager.CanSeeTarget)
			{
				mob.AimPos = aiManager.TargetPos;

				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					if (MoveAroundTarget(aiManager, out Vector3 newPos))
					{
						aiManager.NavMeshAgent.enabled = true;
						aiManager.NavMeshObstacle.enabled = false;
						pos = newPos;
						aiManager.NavMeshAgent.SetDestination(pos);
						aiManager.currentMovementRecoveryTime = aiManager.AgressiveMovementRecoveryTime;
					}
				}

				MoveToLastPos(aiManager);
				AttackAction(aiManager, mob);
			}
		}
	}
}