using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicProp))]
public class Table : Interaction
{
	protected const float flipTorque = 50f;
	protected const float flipLiftingForce = 100f;
	protected const float flipVelocityTreshold = 1e-4f;
	protected const float flipMinimalCooldown = .5f;

	protected float lastFlipTime;
	protected bool flipped = false;

	public DynamicProp Dynamic { get; private set; }

	protected override void Initialzie()
	{
		base.Initialzie();
		Dynamic = Entity as DynamicProp;
	}

	public override bool OnUse(Mob mob)
	{
		Flip(mob.transform.position - Dynamic.Body.position);
		return true;
	}

	public void Flip(Vector3 direction)
	{
		Dynamic.SetFrozen(false);

		Vector3 force = new Vector3(0, flipLiftingForce);
		Dynamic.Body.AddForce(force, ForceMode.Impulse);

		Vector3 torque = Quaternion.Euler(0, 270, 0) * direction;
		torque.Scale(new Vector3(flipTorque, flipTorque, flipTorque));
		Dynamic.Body.AddTorque(torque, ForceMode.Impulse);

		lastFlipTime = Time.time;
		flipped = true;
	}

	public override bool CanBeUsedBy(Mob mob) => !flipped;

	protected override void Tick(float delta)
	{
		base.Tick(delta);
		if (flipped
			&& (Time.time > (lastFlipTime + flipMinimalCooldown))
			&& (Dynamic.Body.velocity.magnitude < flipVelocityTreshold)
			)
		{
			Dynamic.SetFrozen(true);
			flipped = false;
		}
	}
}
