using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoAgressivePattern : AgressivePattern
	{
		private bool test1 = false;
		private bool test2 = false;
		private bool test3 = false;
		private bool test4 = false;
		private bool aiming = false;
		private bool attacking = false;
		[field: SerializeField]
		private float aimingTime = 1.5f;
		[field: SerializeField]
		private float shootingTime = 5;
		[field: SerializeField]
		private float testTime = 2;
		//Нужно получать от оружия
		private int minimumDistanceNeededToAttack = 1;
		private int maximumDistanceNeededToAttack = 10;
		
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			Vector3 targetDirection = aiManager.currentTarget.transform.position - aiManager.transform.position;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

			if (aiming)
			{
				mob.AimPos = AimWithPrediction(aiManager, mob, targetDirection);
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
				mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
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
						aiManager.DebugCube.transform.position = pos;
					}
					MoveToLastPos(aiManager);
					if (aiManager.currentDashRecoveryTime <= 0)
					{
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

		/*
		public override void Tick(AIManager aiManager, Mob mob)
		{
			Vector3 pos;
			Vector3 targetDirection = aiManager.currentTarget.transform.position - aiManager.transform.position;
			mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;
			aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);

			if (test1)
			{
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					aiManager.NavMeshAgent.enabled = true;
					aiManager.NavMeshObstacle.enabled = false;
					pos = new Vector3(20, 0.23f, -25);
					FixPos(pos, out pos);
					aiManager.NavMeshAgent.SetDestination(pos);
					aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
					aiManager.DebugCube.transform.position = pos;
				}
				MoveToLastPos(aiManager);
				if (aiManager.currentDashRecoveryTime <= 0)
				{
					mob.DashAction();
					aiManager.currentDashRecoveryTime = aiManager.DashRecoveryTime;
				}
			}
			else if (test2)
			{
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					aiManager.NavMeshAgent.enabled = true;
					aiManager.NavMeshObstacle.enabled = false;
					pos = new Vector3(20, 0.23f, -35);
					FixPos(pos, out pos);
					aiManager.NavMeshAgent.SetDestination(pos);
					aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
					aiManager.DebugCube.transform.position = pos;
				}
				MoveToLastPos(aiManager);
				if (aiManager.currentDashRecoveryTime <= 0)
				{
					mob.DashAction();
					aiManager.currentDashRecoveryTime = aiManager.DashRecoveryTime;
				}
			}
			else if (test3)
			{
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					aiManager.NavMeshAgent.enabled = true;
					aiManager.NavMeshObstacle.enabled = false;
					pos = new Vector3(5, 0.23f, -35);
					FixPos(pos, out pos);
					aiManager.NavMeshAgent.SetDestination(pos);
					aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
					aiManager.DebugCube.transform.position = pos;
				}
				MoveToLastPos(aiManager);
				if (aiManager.currentDashRecoveryTime <= 0)
				{
					mob.DashAction();
					aiManager.currentDashRecoveryTime = aiManager.DashRecoveryTime;
				}
			}
			else if (test4)
			{
				if (aiManager.currentMovementRecoveryTime <= 0)
				{
					aiManager.NavMeshAgent.enabled = true;
					aiManager.NavMeshObstacle.enabled = false;
					pos = new Vector3(5, 0.23f, -25);
					FixPos(pos, out pos);
					aiManager.NavMeshAgent.SetDestination(pos);
					aiManager.currentMovementRecoveryTime = aiManager.MaxMovementRecoveryTime;
					aiManager.DebugCube.transform.position = pos;
				}
				MoveToLastPos(aiManager);
				if (aiManager.currentDashRecoveryTime <= 0)
				{
					mob.DashAction();
					aiManager.currentDashRecoveryTime = aiManager.DashRecoveryTime;
				}
			}
			else
			{
				StartCoroutine(test(aiManager));
			}
		}*/

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

		private IEnumerator test(AIManager aiManager)
		{
			yield return testSequence(aiManager);
		}

		private IEnumerator testSequence(AIManager aiManager)
		{
			float counter = 0;
			int repeatNumber = 5;
			for (int i = 0; i < repeatNumber; i++)
			{
				test1 = true;
				while (counter < testTime)
				{
					counter += Time.deltaTime;
					yield return null;
				}
				test1 = false;
				test2 = true;

				counter = 0;
				while (counter < testTime)
				{
					counter += Time.deltaTime;
					yield return null;
				}

				test2 = false;
				test3 = true;

				counter = 0;
				while (counter < testTime)
				{
					counter += Time.deltaTime;
					yield return null;
				}

				test3 = false;
				test4 = true;

				counter = 0;
				while (counter < testTime)
				{
					counter += Time.deltaTime;
					yield return null;
				}
				test4 = false;
			}
			aiManager.CurrentRecoveryTime = aiManager.ShootingRecoveryTime;
		}


		public void AttackAction(AIManager aiManager, Mob mob)
		{
			//mob.AimPos = mob.transform.position + targetDirection.normalized * aiManager.distanceFromTarget + Vector3.up * mob.AimHeight;

			mob.UseItem(true);
			if (!mob.ActiveItem.Automatic)
				mob.UseItem(false);
		}
	}

}
