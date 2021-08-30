using System;
using Inventory;
using UnityEngine;

public class Throwable : Item<Throwable>
{
	public event Action OnThrown;
	public event Action OnImpact;

	[field: SerializeField]
	public float ThrowingSpeed { get; private set; } = 25f;
	public int Amount { get; protected set; }

	public override bool Fire(Vector3 target)
	{
		if (!base.Fire(target))
			return false;

		if (Amount <= 0)
			return false;
		Amount--;

		Vector3 direction = (target - Owner.transform.position).normalized;

		print($"{Owner} should throw {this} now");

		return true;
	}
}
