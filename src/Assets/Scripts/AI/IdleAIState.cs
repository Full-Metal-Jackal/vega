using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class IdleAIState : AIState
	{
		public ChaseAIState chaseState;
		public override AIState Tick(AIManager aiManager, Mob mob)
		{
			//Look for target
			//Switch to Pursue target state
			//if no target return this	

			Collider[] colliders = Physics.OverlapSphere(transform.position, aiManager.detectionRadius, aiManager.detectionLayer);

			foreach (Collider colliderElem in colliders)
			{
				Mob character = colliderElem.transform.parent.GetComponent<Mob>();
				if (character != null)
				{
					/* TODO
					 *  Check for team. If AI can attack each other.
					 */
					if (character == aiManager.Player)
					{
						Vector3 targetDirection = character.transform.position - transform.position;
						float viewAngle = Vector3.Angle(targetDirection, transform.forward);

						if (viewAngle > aiManager.minDetectionAngle && viewAngle < aiManager.maxDetectionAngle)
						{
							aiManager.currentTarget = character;
						}
					}
				}
			}

			if (aiManager.currentTarget != null)
			{
				return chaseState;
			}
			else
			{
				return this;
			}
		}
	}
}

