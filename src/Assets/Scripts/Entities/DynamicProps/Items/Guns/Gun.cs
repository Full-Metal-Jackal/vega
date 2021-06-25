using UnityEngine;

public class Gun : Inventory.Item
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

	public void Drop()
	{
		if (!Slot)
		{
			Debug.LogWarning($"Multiple drop attempts of {this}!");
			return;
		}

		// <TODO> Change to Game.itemsHolder or Game.Instance.ItemsHolder as soon as we get our Game singleton working.
		transform.SetParent(GameObject.Find("Items").transform);
	}

	protected virtual void OnHolstered()
	{
		// <TODO> Cease reloading.
		Owner.GunSocket.Clear();
	}

	protected virtual void OnDraw()
	{
		if (!Owner)
		{
			Debug.LogWarning($"{this} is attempted to be drawn without owner, this should never happen.");
			return;
		}

		GameObject model = ItemData.PasteModel(Owner.GunSocket.transform);
		const float skeletonScale = .01f;  // <TODO> Investigate the nature of this scaling later; maybe tweak import settings?
		model.transform.localScale = Vector3.one * skeletonScale;
	}

	protected virtual void OnNoAmmo()
	{
		// <TODO> Make a click sound here and may be start reloading.
	}

	public override void Use()
	{
		// <TODO> Holster mob's active gun, then draw this one.
		OnDraw();
	}
}
