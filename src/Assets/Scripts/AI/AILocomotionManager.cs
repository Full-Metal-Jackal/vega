using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public class AILocomotionManager : MonoBehaviour
	{

		AIManager aiManager;
		public Mob currentTarget;
		public LayerMask detectionLayer;
		public Mob player;

		private void Awake()
		{
			aiManager = GetComponent<AIManager>();
		}
		public void HandleDetection()
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, aiManager.detectionRadius, detectionLayer);

			for (int i = 0; i < colliders.Length; i++)
			{
				Mob character = colliders[i].transform.parent.GetComponent<Mob>();

				if (character != null)
				{
					/* TODO
					 *  Check for team. If AI can attack each other.
					 */
					if (character == player)
					{
						Vector3 targetDirection = character.transform.position - transform.position;
						float viewAngle = Vector3.Angle(targetDirection, transform.forward);

						if (viewAngle > aiManager.minDetectionAngle && viewAngle < aiManager.maxDetectionAngle)
						{
							currentTarget = character;
						}
					}
				}
			}
		}
	}
}

