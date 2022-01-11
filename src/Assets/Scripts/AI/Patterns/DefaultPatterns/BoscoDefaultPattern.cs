using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoDefaultPattern : DefaultPattern
	{
		private bool charging = false;
		private bool attacking = false;
		private Vector3 targetMovementDir;
		private float angle = 0.4f;  //Basicaly speed
		//Нужно получать от оружия
		private int minimumDistanceNeededToAttack = 1;
		private int maximumDistanceNeededToAttack = 10;
		[field : SerializeField]
		private float chargingTime;
		[field: SerializeField]
		private float attackingTime;
		private int dir;

		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			Vector3 targetDirection = aiManager.currentTarget.transform.position - aiManager.transform.position;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

			if (charging)
			{
				// + вызов анимации или еще чего
				mob.AimPos = aiManager.currentTarget.transform.position + Vector3.up * mob.AimHeight;
				targetMovementDir = aiManager.currentTarget.GetComponent<Rigidbody>().velocity.normalized;
				Vector3 predictedTargetDir = aiManager.currentTarget.transform.position - aiManager.transform.position + targetMovementDir;
				dir = Vector3.SignedAngle(predictedTargetDir, targetDirection, Vector3.up) < 0 ? -1 : 1;
				aiManager.movement = Vector3.zero;
			}
			else if (attacking)
			{
				aiManager.movement = Vector3.zero;
				AttackAction(aiManager, mob, dir);
			}
			else
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
				aiManager.DebugCube.transform.position = mob.AimPos;
				mob.UseItem(false);

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
					MoveToLastPos(aiManager);
				}

				if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= maximumDistanceNeededToAttack && aiManager.distanceFromTarget >= minimumDistanceNeededToAttack && aiManager.CanSeeTarget)
				{
					charging = true;
					StartCoroutine(waiter(aiManager));
				}
			}		
		}

		private IEnumerator attackSequence(AIManager aiManager)
		{
			float counter = 0;
			while (counter < chargingTime)
			{
				counter += Time.deltaTime;
				yield return null;
			}
			charging = false;
			attacking = true;

			counter = 0;
			while (counter < attackingTime)
			{
				counter += Time.deltaTime;
				yield return null;
			}
			attacking = false;

			aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
		}

		private IEnumerator waiter(AIManager aiManager)
		{
			yield return attackSequence(aiManager);
		}

		public void AttackAction(AIManager aiManager, Mob mob, int dir)
		{
			mob.AimPos = RotatePointOnAngle(mob.AimPos, mob.transform.position, angle, dir) + Vector3.up * mob.AimHeight;

			mob.UseItem(true);
			if (!mob.ActiveItem.Automatic)
				mob.UseItem(false);
		}
	}
}
