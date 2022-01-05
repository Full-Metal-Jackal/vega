using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoDefaultPattern : DefaultPattern
	{
		private bool charging = false;
		private bool attacking = false;
		//Нужно получать от оружия
		private int minimumDistanceNeededToAttack = 1;
		private int maximumDistanceNeededToAttack = 10;

		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			Vector3 targetDirection = aiManager.currentTarget.transform.position - aiManager.transform.position;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

			if (charging)
			{
				// + вызов анимации или еще чего
				aiManager.movement = Vector3.zero;
			}
			else if (attacking)
			{
				aiManager.movement = Vector3.zero;
				AttackAction(aiManager, mob, targetDirection);
			}
			else
			{
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
				aiManager.DebugCube.transform.position = mob.AimPos;
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					//DashInRandomDirection(aiManager, mob);
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
					StartCoroutine(waiter(aiManager, 3));
				}
			}		
		}

		private IEnumerator attackSequence(float waitTime, AIManager aiManager)
		{
			float counter = 0;
			while (counter < waitTime)
			{
				counter += Time.deltaTime;
				yield return null;
			}
			charging = false;
			attacking = true;

			counter = 0;
			while (counter < waitTime)
			{
				counter += Time.deltaTime;
				yield return null;
			}
			attacking = false;

			aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
		}

		private IEnumerator waiter(AIManager aiManager, float waitTime)
		{
			yield return attackSequence(waitTime, aiManager);
		}

		public void AttackAction(AIManager aiManager, Mob mob, Vector3 targetDirection)
		{
			mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;

			aiManager.DebugCube.transform.position = mob.AimPos;

			mob.UseItem(true);
			if (!mob.ActiveItem.Automatic)
				mob.UseItem(false);
		}
	}

}
