using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class BoscoDeffensivePattern : DeffensivePattern
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
			else if (dashing)
			{
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					print("Should checking");
					aiManager.movement = Vector3.zero;
					if (RandomMovementPos(aiManager, targetDirection, out Vector3 newPos))
					{
						print("Should dashing");
						aiManager.NavMeshAgent.enabled = true;
						aiManager.NavMeshObstacle.enabled = false;
						pos = newPos;
						aiManager.NavMeshAgent.SetDestination(pos);
						aiManager.DebugCube.transform.position = pos;
						aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
						//DashInRandomDirection(aiManager, mob);
					}
					MoveToLastPos(aiManager);
					mob.DashAction();
				}
			}
			else
			{
				aiManager.movement = Vector3.zero;
				mob.AimPos = mob.transform.position;
				aiManager.DebugCube.transform.position = mob.AimPos;
				aiManager.NavMeshAgent.enabled = false;
				aiManager.NavMeshObstacle.enabled = true;
				charging = true;
				StartCoroutine(waiter(aiManager));
			}
		}

		private IEnumerator attackSequence( AIManager aiManager)
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
		public void AttackAction(AIManager aiManager, Mob mob, Vector3 targetDirection)
		{
			//mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;

			mob.UseItem(true);
			if (!mob.ActiveItem.Automatic)
				mob.UseItem(false);
		}
	}
}

