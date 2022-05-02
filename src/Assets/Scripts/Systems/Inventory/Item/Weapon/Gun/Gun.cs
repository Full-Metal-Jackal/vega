using System;
using Inventory;
using UnityEngine;

public class Gun : Weapon
{
	public event Action OnAfterFire;
	public event Action OnAfterReloaded;

	public virtual Transform Barrel { get; protected set; }

	public override Vector3 AimOrigin => Barrel.position;

	[field: SerializeField]
	public GunSfxData SoundEffects { get; protected set; }

	[field: SerializeField]
	public Projectile ProjectilePrefab { get; private set; }

	[field: SerializeField]
	public float ProjectileSpeed { get; private set; } = 50f;

	[field: SerializeField]
	public float Spread { get; private set; } = 0f;

	[field: SerializeField]
	public int ClipSize { get; protected set; } = 8;

	/// <summary>
	/// Fire rate in shots per minute
	/// </summary>
	[field: SerializeField]
	public float FireRate { get; protected set; } = 120;

	/// <summary>
	/// Delay between shots.
	/// </summary>
	public virtual float FireDelay => 60 / FireRate;

	protected float currentFireDelay = 0;

	public int AmmoCount { get; protected set; }

	public bool IsReloading { get; protected set; } = false;
	
	public override bool CanFire => !IsReloading && currentFireDelay <= 0;

	public override bool CanReload => AmmoCount < ClipSize;

	public override bool IsAimable => true;

	public override string SlotText =>
		$"{AmmoCount}/{ClipSize}";

	protected override void Awake()
	{
		base.Awake();
		AmmoCount = ClipSize;
	}

	protected override void Equip()
	{
		base.Equip();

		if (!SoundEffects && ItemData)
			SetupSfx(); 

		if (Model is GunModelData gunModel && !(Barrel = gunModel.Barrel))
			Debug.LogWarning($"{this} has invalid GunModelData: couldn't locate Barrel transform.");
	}

	protected void SetupSfx()
	{
		if (!(SoundEffects = ItemData.PasteSfx(Model.transform) as GunSfxData))
			Debug.LogError($"{this} has no GunSfxData assigned.");
	}

	public override bool Reload()
	{
		if (!base.Reload())
			return false;

		AmmoCount = ClipSize;
		OnAfterReloaded?.Invoke();
		UpdateSlotText();

		return true;
	}

	public override void SingleUse(Vector3 target) =>
		Fire(target);

	protected virtual bool Fire(Vector3 target)
	{
		// Use (target - Owner.transform.position) for shooting parallel to AimDir
		// Use (target - Barrel.position) for shooting directly at the cursor
		Vector3 direction = (target - Barrel.position);
		direction.y = 0f; // <TODO> Change if causes visual inaccuracy.
		direction.Normalize();

		if (!PreFire(ref direction))
			return false;

		Projectile projectile = CreateProjectile(Damage);
		projectile.transform.position = Barrel.position;
		projectile.transform.forward = direction;
		projectile.Body.AddForce(direction * ProjectileSpeed, ForceMode.VelocityChange);

		PostFire(direction, projectile);

		return true;
	}

	public virtual bool PreFire(ref Vector3 direction)
	{
		if (!ConsumeAmmo())
		{
			OnNoAmmo();
			return false;
		}

		if (Spread > 0)
			direction = Quaternion.AngleAxis(
				UnityEngine.Random.Range(-Spread, Spread),
				Barrel.up
			) * direction;


		return true;
	}

	/// <summary>
	/// Tries to consume the gun's ammo.
	/// </summary>
	/// <returns>true if the ammo has been consumed successfully, false otherwise.</returns>
	public virtual bool ConsumeAmmo()
	{
		if (AmmoCount <= 0)
			return false;

		AmmoCount--;
		return true;
	}

	public virtual void PostFire(Vector3 direction, Projectile projectile)
	{
		currentFireDelay = FireDelay;

		SoundEffects.Play(SoundEffects.Fire);
		OnAfterFire?.Invoke();
		UpdateSlotText();
	}

	public virtual Projectile CreateProjectile(Damage damage)
	{
		Projectile projectile = Instantiate(ProjectilePrefab);
		if (!projectile)
			throw new Exception($"{this} received an invalid projectile prefab: {ProjectilePrefab}");

		projectile.Setup(Owner, damage);

		return projectile;
	}

	protected virtual void OnNoAmmo()
	{
		SoundEffects.Play(SoundEffects.DryFire);
		// <TODO> May be start reloading here?
		if (!Owner.IsPlayer)
			Owner.Reload();
	}

	protected virtual void Update()
	{
		if (currentFireDelay > 0)
			currentFireDelay -= Time.deltaTime;
		else if (Owner && Automatic && IsTriggerHeld)
			SingleUse(Owner.AimPos);
	}
}
