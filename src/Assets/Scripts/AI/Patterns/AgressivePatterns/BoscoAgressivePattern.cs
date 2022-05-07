using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoAgressivePattern : AgressivePattern
	{
		private bool aiming = false;
		private bool attacking = false;
		[field: SerializeField]
		private float aimingTime = 1.5f;
		[field: SerializeField]
		private float shootingTime = 5;
		//Нужно получать от оружия
		private int minimumDistanceNeededToAttack = 1;
		private int maximumDistanceNeededToAttack = 10;
		
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

			if (aiming)
			{
				mob.AimPos = AimWithPrediction(aiManager, mob);
				// + вызов анимации или еще чего
				aiManager.movement = Vector3.zero;
			}
			else if (attacking)
			{
				aiManager.movement = Vector3.zero;
				AttackAction(aiManager, mob);
			}
			else
			{
				mob.AimPos = mob.transform.position + aiManager.DefaultTargetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
				mob.UseItem(false);
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
					MoveToLastPos(aiManager);
					if (aiManager.currentDashRecoveryTime <= 0)
					{
						Vector3 targetMovementDir = aiManager.currentTarget.GetComponent<Rigidbody>().velocity.normalized;
						targetMovementDir = mob.transform.position - targetMovementDir;
						mob.SnapTurnTo(new Vector3(targetMovementDir.x, mob.AimPos.y, targetMovementDir.z));
						mob.DashAction();
						aiManager.currentDashRecoveryTime = aiManager.DashRecoveryTime;
					}
				}

				if (aiManager.CurrentRecoveryTime <= 0 && aiManager.distanceFromTarget <= maximumDistanceNeededToAttack && aiManager.distanceFromTarget >= minimumDistanceNeededToAttack && aiManager.CanSeeTarget)
				{
					StartCoroutine(waiter(aiManager));
				}
			}
		} 

		private IEnumerator attackSequence(AIManager aiManager)
		{
			float counter = 0;
			int repeatNumber = 5;
			for (int i = 0; i < repeatNumber; i++)
			{ 
				aiming = true;
				while (counter < aimingTime)
				{
					counter += Time.deltaTime;
					yield return null;
				}
				aiming = false;
				attacking = true;

				counter = 0;
				while (counter < shootingTime)
				{
					counter += Time.deltaTime;
					yield return null;
				}
				attacking = false;
			}
			aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
		}

		private IEnumerator waiter(AIManager aiManager)
		{
			yield return attackSequence(aiManager);
		}

		public override void AttackAction(AIManager aiManager, Mob mob)
		{
			mob.UseItem(true);
			if (!mob.ActiveItem.Automatic)
				mob.UseItem(false);
		}
	}
}
