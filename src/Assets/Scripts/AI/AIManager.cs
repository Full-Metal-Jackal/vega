using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class AIManager : MobController
	{
		AILocomotionManager aiLocomotionManager;
		public bool IsPerfomingAction {get; private set;}

		public AIAttackAction[] aiAttacks;
		private AIAttackAction currentAttack;

		private float currentRecoveryTime = 0;

		[Header("A.I Settings")]
		public float detectionRadius = 5;
		public float maxDetectionAngle = 50;
		public float minDetectionAngle = -50;

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
				aiLocomotionManager.DistanceFromTarget = Vector3.Distance(aiLocomotionManager.currentTarget.transform.position, transform.position);
			}
			if (aiLocomotionManager.currentTarget == null)
			{
				aiLocomotionManager.HandleDetection();
			}
			else if (aiLocomotionManager.DistanceFromTarget > aiLocomotionManager.stoppingDistance)
			{
				aiLocomotionManager.HandleMoveToTarget(delta);
			}
			else if (aiLocomotionManager.DistanceFromTarget <= aiLocomotionManager.stoppingDistance)
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
			else if (IsPerfomingAction)
			{
				IsPerfomingAction = false;
			}
		}
#region Attacks
		private void AssignNewAttack()
		{
			print("Wha");
			Vector3 targetDirection = aiLocomotionManager.currentTarget.transform.position - transform.position;
			float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
			aiLocomotionManager.DistanceFromTarget = Vector3.Distance(aiLocomotionManager.currentTarget.transform.position, transform.position);

			if (currentAttack != null)
			{
				return;
			}

			int maxScore = 0;

			foreach (AIAttackAction aiAttackAction in aiAttacks)
			{
				if (aiLocomotionManager.DistanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack
					&& aiLocomotionManager.DistanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
				{
					if (viewableAngle <= aiAttackAction.maximumAttackAngle && viewableAngle >= aiAttackAction.minimumAttackAngle)
					{
						maxScore += aiAttackAction.attackScore;
					}
				}
			}

			int randomValue = Random.Range(0, maxScore);
			int tmpScore = 0;

			foreach (AIAttackAction aiAttackAction in aiAttacks)
			{
				if (aiLocomotionManager.DistanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack
					&& aiLocomotionManager.DistanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
				{
					if (viewableAngle <= aiAttackAction.maximumAttackAngle && viewableAngle >= aiAttackAction.minimumAttackAngle)
					{
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
			if (IsPerfomingAction)
			{
				return;
			}
			if (currentAttack == null)
			{
				AssignNewAttack();
			}
			else
			{
				IsPerfomingAction = true;
				currentRecoveryTime = currentAttack.recoveryTime;
				//TODO Кусок говнокода, как замена воспроизведения атаки.
				Debug.Log("ATTTAAAACK ANIMATION FOR " + currentAttack.attackName);
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

