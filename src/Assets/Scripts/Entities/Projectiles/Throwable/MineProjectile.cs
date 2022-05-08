using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProjectile : GrenadeProjectile, IDamageable
{
	[SerializeField]
	private float armDelay = 2f;
	private bool armed = false;

	[SerializeField]
	private Event onTriggered;
	
	[SerializeField]
	private float triggerRadius = 1f;
	private bool triggered = false;

	Collider[] detectionResults = new Collider[1];

	private LayerMask triggerMask;

	protected override void Awake()
	{
		base.Awake();

		triggerMask = LayerMask.GetMask(new string[] { "Mobs" });
	}

	protected override void Update()
	{
		float delta = Time.deltaTime;

		if (!armed)
		{
			if ((armDelay -= delta) < 0)
				Arm();
			return;
		}

		if (!triggered)
		{
			triggered = Physics.OverlapSphereNonAlloc(
				transform.position, triggerRadius, detectionResults, layerMask: triggerMask
			) > 0;
			return;
		}

		base.Update();
	}

	protected override void Explode()
	{
		base.Explode();

		print($"{this} exploded");
	}

	protected virtual void Arm()
	{
		Debug.Log($"{this} has been armed");

		armed = true;
	}

	public void Trigger()
	{
		triggered = true;
	}

	public void TakeDamage(Damage damage)
	{
		Trigger();
	}
}
