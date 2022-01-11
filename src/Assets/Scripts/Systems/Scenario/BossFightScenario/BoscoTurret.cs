using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoscoTurret : MonoBehaviour
{
	[field: SerializeField]
	private Mob possesed;
	[field: SerializeField]
	private Mob target;
	[field: SerializeField]
	private float attackRecoveryTime;
	[field: SerializeField]
	private float valleyRecoveryTime;
	[field: SerializeField]
	private float stateSelectionRecoveryTime;
	[field: SerializeField]
	private float durationOfRecoveryStateTime;
	[field: SerializeField]
	private float machinegunAttackInterval;
	[field: SerializeField]
	private Spawner spawner;


	private bool attacking = false;
	private bool reloading = false;
	private float currentAttackRecoveryTime;
	private float currentValleyRecoveryTime;
	private float currentMachinegunRecoveryTime;
	private float currentDurationOfRecoveryStateTime;
	private float curentStateSelectionRecoveryTime;
	private int stateNumber = 0;
	private int stateOrder;

	public bool IsValleyState = false;
	public bool IsMachinegunState = false;
	public bool IsRecoveryState = false;
	public bool IsLoopStarted = false;

	void Update()
	{
		if (curentStateSelectionRecoveryTime <= 0)
		{
			PickState();
		}
		if (IsRecoveryState)
		{
			print("Recovery State");
		}
		if (IsMachinegunState)
		{
			Vector3 targetDirection = target.transform.position - possesed.transform.position;
			possesed.AimPos = target.transform.position + Vector3.up * possesed.AimHeight;
			possesed.TurnTo(Time.deltaTime, possesed.AimDir);
			if (currentMachinegunRecoveryTime <= 0)
			{
				if (!attacking)
					StartCoroutine(waiter(machinegunAttackInterval));
				else
					possesed.UseItem(true);
			}
		}
		if (IsValleyState)
		{
			if (currentValleyRecoveryTime <= 0)
			{
				LaunchValley();
				currentValleyRecoveryTime = valleyRecoveryTime;
			}
		}

		HandleRecoveryTime();
	}

	private void PickState()
	{
		if (!IsLoopStarted)
		{
			stateOrder = Random.Range(0, 2);
			IsLoopStarted = true;
		}
		if (stateOrder == 0)
		{
			switch (stateNumber)
			{
			case 0:
				IsValleyState = false;
				IsMachinegunState = true;
				stateNumber++;
				break;
			case 1:
				IsMachinegunState = false;
				IsValleyState = true;
				stateNumber++;
				break;
			case 2:
				IsMachinegunState = true;
				IsValleyState = true;
				stateNumber ++;
				break;
			case 3:
				IsMachinegunState = false;
				IsValleyState = false;
				stateNumber = 0;
				IsLoopStarted = false;
				break;
			}
		}
		else if (stateOrder == 1)
		{
			switch (stateNumber)
			{
			case 0:
				IsValleyState = true;
				IsMachinegunState = false;
				stateNumber++;
				break;
			case 1:
				IsMachinegunState = true;
				IsValleyState = false;
				stateNumber++;
				break;
			case 2:
				IsMachinegunState = true;
				IsValleyState = true;
				stateNumber++;
				break;
			case 3:
				IsMachinegunState = false;
				IsValleyState = false;
				stateNumber = 0;
				IsLoopStarted = false;
				break;
			}
		}
		curentStateSelectionRecoveryTime = stateSelectionRecoveryTime;
	}

	private IEnumerator attackSequence(float time)
	{
		attacking = true;
		float counter = 0;
		while (counter < time)
		{
			counter += Time.deltaTime;
			yield return null;
		}

		currentAttackRecoveryTime = attackRecoveryTime;
		attacking = false;
		possesed.UseItem(false);
	}

	private IEnumerator waiter(float time)
	{
		yield return attackSequence(time);
	}

	private void HandleRecoveryTime()
	{
		if (currentAttackRecoveryTime > 0)
		{
			currentAttackRecoveryTime -= Time.deltaTime;
		}

		if (currentMachinegunRecoveryTime > 0)
		{
			currentMachinegunRecoveryTime -= Time.deltaTime;
		}

		if (currentValleyRecoveryTime > 0)
		{
			currentValleyRecoveryTime -= Time.deltaTime;
		}

		if (curentStateSelectionRecoveryTime > 0)
		{
			curentStateSelectionRecoveryTime -= Time.deltaTime;
		}
	}

	//spawn new dangerous zone
	private void LaunchValley()
	{
		spawner.SpawnOn(target.transform);
	}
}


