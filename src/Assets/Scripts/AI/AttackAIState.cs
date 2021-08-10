using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class AttackAIState : AIState
	{
		public CombatStanceAIState combateStance;

		private AIAttackAction currentAttack;

		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Select attack base on attack score
			//if selected attack not able to be used - select new attack
			//if attack is viable, stop movement and attack target
			//set recovery timer to attacks recovery time
			//return combat stance state
			
			if (aiManager.IsPerfomingAction)
			{
				return combateStance;
			}

			if (currentAttack != null)
			{
				if (aiManager.DistanceFromTarget <= currentAttack.minimumDistanceNeededToAttack)
				{
					return this;
				}
				else if (aiManager.DistanceFromTarget <= currentAttack.maximumDistanceNeededToAttack)
				{
					if (aiManager.viewableAngle <= currentAttack.maximumAttackAngle &&
						aiManager.viewableAngle >= currentAttack.minimumAttackAngle)
					{
						if (aiManager.CurrentRecoveryTime <= 0 && aiManager.IsPerfomingAction == false)
						{
							aiManager.movement = Vector3.zero;
							aiManager.IsPerfomingAction = true;
							//TODO Кусок говнокода, как замена воспроизведения атаки.
							Debug.Log("ATTTAAAACK ANIMATION FOR " + currentAttack.attackName);
							aiManager.CurrentRecoveryTime = currentAttack.recoveryTime;
							currentAttack = null;
							return combateStance;
						}
					}
				}
			}
			else
			{
				AssignNewAttack(aiManager);
			}

			return combateStance;
		}

		private void AssignNewAttack(AIManager aiManager)
		{
			Vector3 targetDirection = aiManager.currentTarget.transform.position - transform.position;
			float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
			aiManager.DistanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, transform.position);

			if (currentAttack != null)
			{
				return;
			}

			int maxScore = 0;

			foreach (AIAttackAction aiAttackAction in aiManager.aiAttacks)
			{
				if (aiManager.DistanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack
					&& aiManager.DistanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
				{
					if (viewableAngle <= aiAttackAction.maximumAttackAngle && viewableAngle >= aiAttackAction.minimumAttackAngle)
					{
						maxScore += aiAttackAction.attackScore;
					}
				}
			}

			int randomValue = Random.Range(0, maxScore);
			int tmpScore = 0;

			foreach (AIAttackAction aiAttackAction in aiManager.aiAttacks)
			{
				if (aiManager.DistanceFromTarget <= aiAttackAction.maximumDistanceNeededToAttack
					&& aiManager.DistanceFromTarget >= aiAttackAction.minimumDistanceNeededToAttack)
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
	}
}
