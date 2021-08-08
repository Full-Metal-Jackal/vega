using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class IdleAIState : AIState
	{
		public ChaseAIState chaseState;

		[field: SerializeField]
		public bool CanSeePlayer { get; set; }

		public override AIState RunCurrentState()
		{
			if (CanSeePlayer)
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

