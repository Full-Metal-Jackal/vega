using UnityEngine;

public class Gun : MonoBehaviour
{
	public bool Initialized { get; private set; } = false;

	[field: SerializeField]
	public int ClipSize { get; protected set; } = 8;

	[field: SerializeField]
	public Transform Barrel { get; private set; }

	[field: SerializeField]
	public GameObject ProjectilePrefab { get; private set; }

	public int AmmoCount { get; protected set; }

	public bool IsReloading { get; protected set; } = false;
	
	public virtual bool CanFire => !IsReloading;

	private void Awake()
	{
		Initialize();
	}

	public void Initialize()
	{
		if (Initialized)
			throw new System.Exception();

		AmmoCount = ClipSize;

		Initialized = true;
		return;
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

	public virtual void Fire() => Fire(Barrel.forward);
	public virtual void FireAt(Vector3 position) => Fire(position - Barrel.position);

	public virtual void Fire(Vector3 direction)
	{
		PreFire(direction);

		Projectile projectile = CreateProjectile();
		projectile.transform.position = Barrel.position;
		projectile.transform.forward = direction;

		PostFire(direction, projectile);
	}

	public virtual void PreFire(Vector3 direction)
	{
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
}
