using Inventory;
using UnityEngine;

public class Gun : Item
{
	public int ClipSize { get; protected set; } = 8;

	public Transform Barrel { get; protected set; }
	public GunSfxData SoundEffects { get; protected set; }

	[field: SerializeField]
	public GameObject ProjectilePrefab { get; private set; }

	[field: SerializeField]
	public float ProjectileSpeed { get; private set; } = 50f;

	[field: SerializeField]
	public float Spread { get; private set; } = 0f;

	[field: SerializeField]
	public float Damage { get; private set; } = 10f;

	[field: SerializeField]
	public float Mass { get; private set; } = 2f;

	public int AmmoCount { get; protected set; }

	public bool IsReloading { get; protected set; } = false;
	
	public override bool CanFire => !IsReloading;

	public override bool IsAimable => true;

	protected override void Initialize()
	{
		base.Initialize();
		AmmoCount = ClipSize;
	}

	protected override void Equip()
	{
		base.Equip();

		if (!(SoundEffects = (GunSfxData)ItemData.PasteSfx(Model.transform)))
			Debug.LogError($"{this} has no GunSfxData assigned.");

		if (!(Model is GunModelData gunModel) || !gunModel.Barrel)
		{
			Debug.LogWarning($"{this} has invalid GunModelData: couldn't locate Barrel transform.");
			Barrel = Model.transform;
			return;
		}

		Barrel = gunModel.Barrel;
	}

	public void OnReloadBegin() =>
		IsReloading = true;

	public void OnReloadEnd()
	{
		Reload();
		IsReloading = false;
	}

	public virtual void Reload() =>
		AmmoCount = ClipSize;

	public override bool Fire(Vector3 target)
	{
		if (!base.Fire(target))
			return false;

		Vector3 direction = (target - Barrel.position).normalized;

		if (!PreFire(ref direction))
			return false;

		Projectile projectile = CreateProjectile();
		projectile.transform.position = Barrel.position;
		projectile.transform.forward = direction;
		projectile.Body.AddForce(direction * ProjectileSpeed, ForceMode.VelocityChange);

		PostFire(direction, projectile);

		return true;
	}

	public virtual bool PreFire(ref Vector3 direction)
	{
		if (AmmoCount <= 0)
		{
			OnNoAmmo();
			return false;
		}

		direction = Quaternion.AngleAxis(
			Random.Range(-Spread, Spread),
			Barrel.up
		) * direction;

		AmmoCount--;

		return true;
	}

	public virtual void PostFire(Vector3 direction, Projectile projectile)
	{
		SoundEffects.Play(SoundEffects.Fire);
	}

	public virtual Projectile CreateProjectile()
	{
		if (!Instantiate(ProjectilePrefab).TryGetComponent(out Projectile projectile))
			throw new System.Exception($"{this} received an invalid projectile prefab: {ProjectilePrefab}");

		projectile.Setup(Owner);
		return projectile;
	}

	protected virtual void OnNoAmmo()
	{
		SoundEffects.Play(SoundEffects.DryFire);
		// <TODO> May be start reloading here?
	}
}
