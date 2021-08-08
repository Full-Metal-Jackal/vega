using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAIState : AIState
{
	public AttackAIState attackState;
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
