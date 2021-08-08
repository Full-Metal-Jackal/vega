using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAIState : AIState
{
	public ChaseAIState chaseState;
	public bool CanSeePlayer { get; set;}

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
