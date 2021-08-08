using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class ChaseAIState : AIState
	{
		public AttackAIState attackState;

		[field: SerializeField]
		public bool IsInAttackRange { get; set; }

		public override AIState RunCurrentState()
		{
			if (IsInAttackRange)
			{
				return attackState;
			}
			else
			{
				return this;
			}
		}
	}
}

