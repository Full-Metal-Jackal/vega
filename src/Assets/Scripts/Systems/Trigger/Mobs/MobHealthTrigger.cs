using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TriggerSystem;

public class MobHealthTrigger : Trigger
{
	[SerializeField]
	private Mob mob;

	[SerializeField]
	private float health = 30f;

	private void Awake()
	{
		mob.OnDamaged += RegisterDeath;
	}

	private void RegisterDeath(Mob mob)
	{
		mob.OnDamaged -= RegisterDeath;

		Activate();
	}
}
