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
	private float machinegunAttackInterval;
	[field: SerializeField]
	private Spawner spawner;


	private bool attacking = false;
	private bool reloading = false;
	private float currentAttackRecoveryTime;
	private float currentValleyRecoveryTime;
	private float currentMachinegunRecoveryTime;

	public bool IsValleyState = false;
	public bool IsMachinegunState = true;

	void Update()
	{
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
	}

	//spawn new dangerous zone
	private void LaunchValley()
	{
		spawner.SpawnOn(target.transform);
	}
}


