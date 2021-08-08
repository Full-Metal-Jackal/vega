using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAIState : AIState
{
	public override AIState RunCurrentState()
	{
		Debug.Log("Attacked ");
		return this;
	}
}
