using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurretBehavior : MonoBehaviour
{
	[field : SerializeField]
	private Mob possesed;
	[field: SerializeField]
	private Mob target;
	[field: SerializeField]
	private float attackRecoveryTime;
	[field: SerializeField]
	private float defaultAttackInterval;
	[field: SerializeField]
	private float specialAttackInterval;

	private bool attacking = false;
	private float currentAttackRecoveryTime;

	public bool IsDefaultState = true;

	void Update()
	{
		//target dir
		//aim pos
		//time between attacks
		//if default state attack in interval
		//else attack long
		Vector3 targetDirection = target.transform.position - possesed.transform.position;
		possesed.AimPos = target.transform.position + Vector3.up * possesed.AimHeight;
		possesed.TurnTo(Time.deltaTime, possesed.AimDir);
		if (IsDefaultState)
		{
			if (currentAttackRecoveryTime <= 0)
			{
				if (!attacking)
					StartCoroutine(waiter(defaultAttackInterval));
				else
					possesed.UseItem(true);
			}
		}
		else
		{
			if (!attacking)
				StartCoroutine(waiter(specialAttackInterval));
			else
				possesed.UseItem(true);
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
	}
}


