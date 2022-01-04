using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoDefaultPattern : DefaultPattern
	{
		private bool buisy = false;

		public override void Tick(AIManager aiManager, Mob mob)
		{
			DashInRandomDirection(aiManager, mob);
			MoveToLastPos(aiManager);
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

		private IEnumerator prepareForAttack(float waitTime, AIManager aiManager)
		{
			float counter = 0;

			buisy = true;
			while (counter < waitTime)
			{
				//Increment Timer until counter >= waitTime
				counter += Time.deltaTime;
				Debug.Log("We have waited for: " + counter + " seconds");
				//Wait for a frame so that Unity doesn't freeze
				yield return null;
			}
			buisy = false;
		}

		IEnumerator waiter(AIManager aiManager)
		{
			float waitTime = 4;
			yield return prepareForAttack(waitTime, aiManager);
		}

		public override void AttackAction(AIManager aiManager, Mob mob)
		{
			//Нужно получать от оружия
			int minimumDistanceNeededToAttack = 1;
			int maximumDistanceNeededToAttack = 10;

			if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= maximumDistanceNeededToAttack && aiManager.distanceFromTarget >= minimumDistanceNeededToAttack)
			{
				print("Getting ready to attack");

				//aiManager.PerfomeAction(2f); //вынести в константу
				StartCoroutine(waiter(aiManager));
				while (buisy)
				{
					print("test2");
					aiManager.movement = Vector3.zero;
					print("test3");
				}
				print("Ready to attack");
				//aiManager.PerfomeAction(3f); //вынести в константу


				
				while (aiManager.isPerfomingAction)
				{
					print("poof");
					mob.UseItem(true);
					if (!mob.ActiveItem.Automatic)
						mob.UseItem(false);
					aiManager.movement = Vector3.zero;
				}

				aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
			}
		}
	}

}
