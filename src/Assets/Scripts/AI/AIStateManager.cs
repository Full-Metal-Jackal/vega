using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{

	public AIState currentState;
	private void Update()
	{
		
	}

	private void RunStateMachine()
	{
		AIState nextState = currentState?.RunCurrentState();

		if (nextState != null)
		{
			//Switch to the next State
			SwitchState(nextState);
		}
	}

	private void SwitchState(AIState nextState)
	{
		currentState = nextState;
	}
}
