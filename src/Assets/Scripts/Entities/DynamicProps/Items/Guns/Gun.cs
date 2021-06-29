using Inventory;
using UnityEngine;

public class Gun : Item
{
	public int ClipSize { get; protected set; } = 8;

	[field: SerializeField]
	public Transform Barrel { get; private set; }

	[field: SerializeField]
	public GameObject ProjectilePrefab { get; private set; }

	[field: SerializeField]
	public float ProjectileSpeed { get; private set; } = 1200f;

	[field: SerializeField]
	public float Damage { get; private set; } = 10f;

	[field: SerializeField]
	public float Mass { get; private set; } = 2f;

	public int AmmoCount { get; protected set; }

	public bool IsReloading { get; protected set; } = false;
	
	public virtual bool CanFire => !IsReloading;

	protected override void Initialize()
	{
		base.Initialize();
		AmmoCount = ClipSize;
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

	public virtual void Fire() =>
		Fire(Barrel.forward);

	public virtual void FireAt(Vector3 position) =>
		Fire(position - Barrel.position);

	public virtual void Fire(Vector3 direction)
	{
		if (!PreFire(direction))
			return;

		Projectile projectile = CreateProjectile();
		projectile.transform.position = Barrel.position;
		projectile.transform.forward = direction;
		projectile.Body.AddForce(Barrel.forward * ProjectileSpeed, ForceMode.Impulse);

		PostFire(direction, projectile);
	}

	public virtual bool PreFire(Vector3 direction)
	{
		if (AmmoCount <= 0)
		{
			OnNoAmmo();
			return false;
		}

		AmmoCount--;

		return true;
	}

	public virtual void PostFire(Vector3 direction, Projectile projectile)
	{
	}

	public virtual Projectile CreateProjectile()
	{
		if (!Instantiate(ProjectilePrefab).TryGetComponent(out Projectile projectile))
			throw new System.Exception($"{this} received an invalid projectile prefab: {ProjectilePrefab}");

		return projectile;
	}

	protected virtual void OnNoAmmo()
	{
		// <TODO> Make a click sound here and may be start reloading.
	}
}
