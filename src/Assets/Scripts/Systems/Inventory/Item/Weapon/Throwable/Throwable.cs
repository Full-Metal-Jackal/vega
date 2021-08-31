using System;
using Inventory;
using UnityEngine;

public class Throwable : Item<Throwable>
{
	[field: SerializeField]
	public Projectile ProjectilePrefab { get; private set; }
	[field: SerializeField]
	public float ThrowingSpeed { get; private set; } = 25f;

	public int Amount { get; protected set; }
	public int MaxAmount { get; protected set; }

	public float RechargeDelay { get; private set; } = 5f;
	private float rechargeProgress = 0f;

	public override string SlotText => Amount.ToString();

	public override bool Fire(Vector3 target)
	{
		if (!base.Fire(target))
			return false;

		if (Amount <= 0)
			return false;
		Amount--;
		UpdateSlotText();

		Vector3 direction = (target - Owner.transform.position).normalized;

		Projectile projectile = CreateProjectile();
		projectile.transform.position = Owner.ItemSocket.position;
		projectile.transform.forward = direction;
		projectile.Body.AddForce(direction * ThrowingSpeed, ForceMode.VelocityChange);

		return true;
	}

	public virtual Projectile CreateProjectile()
	{
		Projectile projectile = Instantiate(ProjectilePrefab);
		if (!projectile)
			throw new Exception($"{this} received an invalid projectile prefab: {ProjectilePrefab}");

		projectile.Setup(Owner);
		return projectile;
	}

	protected void Update()
	{
		Recharge(Time.deltaTime);
	}

	protected virtual void Recharge(float delta)
	{
		if (Amount >= MaxAmount)
			return;

		float overlap = (rechargeProgress += delta) - RechargeDelay;
		if (overlap < 0f)
			return;

		if (++Amount < MaxAmount)
			rechargeProgress = overlap;
		else
			rechargeProgress = 0f;

		UpdateSlotText();
	}
}
