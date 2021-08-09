using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class AIManager : MobController
	{
		AILocomotionManager aiLocomotionManager;
		public bool isPerfomingAction = true;

		public AIAttackAction[] aiAttacks;
		public AIAttackAction currentAttack;

		[Header("A.I Settings")]
		public float detectionRadius = 5;
		public float maxDetectionAngle = 50;
		public float minDetectionAngle = -50;

		public float currentRecoveryTime = 0;

		protected override void Initialize()
		{
			base.Initialize();
		    aiLocomotionManager = GetComponent<AILocomotionManager>();
	}
		protected override void OnUpdate(float delta)
		{
			HandleCurrentAction(delta);
			HandleRecoveryTime(delta);
		}

		private void HandleCurrentAction(float delta)
		{
			if (aiLocomotionManager.currentTarget != null)
			{
				aiLocomotionManager.distanceFromTarget = Vector3.Distance(aiLocomotionManager.currentTarget.transform.position, transform.position);
			}
			if (aiLocomotionManager.currentTarget == null)
			{
				aiLocomotionManager.HandleDetection();
			}
			else if (aiLocomotionManager.distanceFromTarget > aiLocomotionManager.stoppingDistance)
			{
				aiLocomotionManager.HandleMoveToTarget(delta);
			}
			else if (aiLocomotionManager.distanceFromTarget <= aiLocomotionManager.stoppingDistance)
			{
				//Handle Attack
				AttackTarget();
			}
		}

		private void HandleRecoveryTime(float delta)
		{
			if (currentRecoveryTime > 0)
			{
				currentRecoveryTime -= delta;
			}

			if (isPerfomingAction)
			{
				if (currentRecoveryTime <= 0)
				{
					isPerfomingAction = false;
				}
			}
		}
		#region Attacks
		private void GetNewAttack()
		{
			Vector3 targetDirection = aiLocomotionManager.currentTarget.transform.position - transform.position;
			float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
			aiLocomotionManager.distanceFromTarget = Vector3.Distance(aiLocomotionManager.currentTarget.transform.position, transform.position);

			int maxScore = 0;

			for (int i = 0; i < aiAttacks.Length; i++)
			{
				AIAttackAction aiAttackAction = aiAttacks[i];

				if (aiLocomotionManager.distanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack
					&& aiLocomotionManager.distanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
				{
					if (viewableAngle <= aiAttackAction.maximumAttackAngle && viewableAngle >= aiAttackAction.minimumAttackAngle)
					{
						maxScore += aiAttackAction.attackScore;
					}
				}
			}

			int randomValue = Random.Range(0, maxScore);
			int tmpScore = 0;

			for (int i = 0; i < aiAttacks.Length; i++)
			{
				AIAttackAction aiAttackAction = aiAttacks[i];

				if (aiLocomotionManager.distanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack
					&& aiLocomotionManager.distanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
				{
					if (viewableAngle <= aiAttackAction.maximumAttackAngle && viewableAngle >= aiAttackAction.minimumAttackAngle)
					{
						if (currentAttack != null)
						{
							return;
						}

						tmpScore += aiAttackAction.attackScore;

						if (tmpScore > randomValue)
						{
							currentAttack = aiAttackAction;
						}
					}
				}
			}
		}

		private void AttackTarget()
		{
			if (isPerfomingAction)
			{
				return;
			}
			if (currentAttack == null)
			{
				GetNewAttack();
			}
			else
			{
				isPerfomingAction = true;
				currentRecoveryTime = currentAttack.recoveryTime;
				//TODO Кусок говнокода, как замена воспроизведения атаки.
				print("ATTTAAAACK ANIMATION FOR " + currentAttack.attackName);
				currentAttack = null;
			}
		}

	
		#endregion

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, detectionRadius);
		}
	}
}

