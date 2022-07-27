using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FreezeeAI
{
	public class BoscoDefensivePattern : DefensivePattern
	{
		private bool charging = false;
		private bool attacking = false;
		private bool dashing = false;
		[field: SerializeField]
		private float chargingTime = 1.5f;
		[field: SerializeField]
		private float attackingTime = 0.5f;
		[field: SerializeField]
		private float dashingTime = 3;
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

			if (charging)
			{
				// + вызов анимации или еще чего
				aiManager.Movement = Vector3.zero;
			}
			else if (attacking)
			{
				aiManager.Movement = Vector3.zero;
				AttackAction(aiManager, mob);
			}
			else if (dashing)
			{
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
					if (aiManager.currentDashRecoveryTime <= 0)
					{
						MoveToLastPos(aiManager);
						mob.DashAction();
						aiManager.currentDashRecoveryTime = aiManager.DashRecoveryTime;
					}
					MoveToLastPos(aiManager);
				}
			}
			else
			{
				aiManager.Movement = Vector3.zero;
				mob.AimPos = mob.transform.position;
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				StartCoroutine(waiter(aiManager));
			}
		}

		private IEnumerator attackSequence( AIManager aiManager)
		{
			charging = true;
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
			dashing = true;

			counter = 0;
			while (counter < dashingTime)
			{
				counter += Time.deltaTime;
				yield return null;
			}

			dashing = false;
			aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
		}

		private IEnumerator waiter(AIManager aiManager)
		{
			yield return attackSequence(aiManager);
		}

		// Использовать стан
		public override void AttackAction(AIManager aiManager, Mob mob)
		{
			mob.UseItem(true);
			if (!mob.ActiveItem.Automatic)
				mob.UseItem(false);
		}
	}
}

