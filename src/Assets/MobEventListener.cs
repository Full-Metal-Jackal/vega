using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEventListener : MonoBehaviour
{
	private Mob mob;
	private void Awake()
	{
		mob = transform.parent.GetComponent<Mob>();
	}

	public void OnDodgeRollBegin() => mob.OnDodgeRoll();

	public void OnDodgeRollEnd() => mob.OnDodgeRollEnd();
}
