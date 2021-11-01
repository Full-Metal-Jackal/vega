using System;
using Inventory;
using UnityEngine;

public class Throwable : Item<Throwable>
{
	[field: SerializeField]
	public Projectile ProjectilePrefab { get; private set; }

	[field: SerializeField]
	public float ThrowingSpeed { get; private set; } = 8f;

	/// <summary>
	/// The torque applied to the thrown projectile.
	/// </summary>
	[field: SerializeField]
	public Vector3 ThrowingTorque { get; private set; } = Vector3.zero;

	/// <summary>
	/// How much, in degrees, the throwing direction is elevated.
	/// If 0, the projectile will be thrown straight froward.
	/// </summary>
	[field: SerializeField]
	public float ThrowAngle { get; private set; } = 0f;

	public int Amount { get; protected set; } = 1;
	[field: SerializeField]
	public int MaxAmount { get; protected set; } = 3;
	public override bool CanFire => base.CanFire && Amount > 0;

	public float RechargeDelay { get; private set; } = 5f;
	private float rechargeProgress = 0f;

	public override string SlotText => Amount.ToString();

	[field: SerializeField]
	public Damage Damage { get; private set; }

	private ItemModelData model;

	public override void SingleUse(Vector3 target)
	{
		Throw(target);
		SetTrigger(target, false);
	}

	protected bool Throw(Vector3 target)
	{
		Amount--;
		UpdateSlotText();

		Vector3 direction = (target - Owner.transform.position).normalized;

		Projectile projectile = CreateProjectile();
		projectile.transform.position = Owner.ItemSocket.position;
		projectile.transform.forward = direction;
		projectile.Body.AddForce(Owner.Body.velocity + direction * ThrowingSpeed, ForceMode.VelocityChange);
		projectile.Body.AddRelativeTorque(ThrowingTorque, ForceMode.VelocityChange);

		return true;
	}

	public virtual Projectile CreateProjectile()
	{
		Projectile projectile = Instantiate(ProjectilePrefab);
		if (!projectile)
			throw new Exception($"{this} received an invalid projectile prefab: {ProjectilePrefab}");

		projectile.Body.mass = Mass;
		model.transform.SetParent(projectile.transform, false);
		ItemData.PasteCollisions(projectile.transform);
		projectile.Setup(Owner, Damage);

		return projectile;
	}

	protected virtual void Update()
	{
		print(IsTriggerHeld);
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

	// Creates model to reparent to the projectile later.
	public void SetupModel(Transform parent) =>
		model = ItemData.PasteModel(parent);
	public void SetupModel() => SetupModel(transform);
}
