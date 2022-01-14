using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProjectile : GrenadeProjectile
{
	[SerializeField]
	private float activationDelay = 2f;
	private bool activated = false;

	[SerializeField]
	private Event onTriggered;

	[SerializeField]
	private float triggerRadius = 1f;
	private bool triggered = false;

	protected override void Update()
	{
		float delta = Time.deltaTime;

		if (!activated)
		{
			activated = (activationDelay -= delta) < 0;
			return;
		}

		if (!triggered)
			return;
		base.Update();
	}

	public void Trigger()
	{
		triggered = true;
	}
}
