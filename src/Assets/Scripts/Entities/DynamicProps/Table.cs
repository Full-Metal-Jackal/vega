using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : DynamicProp, IInteractable
{
	public bool Selectable { get; set; } = true;

	protected const float flipTorque = 50f;
	protected const float flipLiftingForce = 100f;
	protected const float flipVelocityTreshold = 1e-4f;
	protected const float flipMinimalCooldown = .5f;

	protected float lastFlipTime;
	protected bool flipped = false;

	public bool OnUse(Mob mob)
	{
		Flip(mob.transform.position - transform.position);
		return true;
	}

	public void Flip(Vector3 direction)
	{
		SetFrozen(false);

		Vector3 force = new Vector3(0, flipLiftingForce);
	    Body.AddForce(force, ForceMode.Impulse);

		Vector3 torque = Quaternion.Euler(0, 270, 0) * direction;
		torque.Scale(new Vector3(flipTorque, flipTorque, flipTorque));
		Body.AddTorque(torque, ForceMode.Impulse);

		lastFlipTime = Time.time;
		flipped = true;
	}

	public bool CanBeUsedBy(Mob mob) => !flipped;

	protected override void Tick(float delta)
	{
		base.Tick(delta);
		if (flipped
			&& (Time.time > (lastFlipTime + flipMinimalCooldown))
			&& (Body.velocity.magnitude < flipVelocityTreshold)
			)
		{
			SetFrozen(true);
			flipped = false;
		}
	}
}
