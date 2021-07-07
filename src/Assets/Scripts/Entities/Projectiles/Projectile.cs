using UnityEngine;

public class Projectile : DynamicEntity
{
	[field: SerializeField]
	public float Damage { get; set; } = 10f;

	/// <summary>
	/// How many seconds this projectile lives until being cleaned up.
	/// Provide 0 to disable automatic cleanup.
	/// </summary>
	[SerializeField]
	private float lifespan = 5f;

	/// <summary>
	/// How much time left till this projectile dies.
	/// </summary>
	public float LifeSpan
	{
		get => lifespan;
		protected set
		{
			if ((lifespan = value) <= 0)
				Suicide();
		}
	}

	/// <summary>
	/// Who (or what) shot Kennedy.
	/// </summary>
	public Entity Source { get; protected set; }

	[field: SerializeField]
	public ProjecitleType Type { get; private set; } = ProjecitleType.Kinetic;

	public void Setup(Entity source)
	{
		Source = source;
	}

	private void OnCollisionEnter(Collision collision) => OnHit(collision.gameObject);

	public virtual void OnHit(GameObject other)
	{
		Suicide();
		if (other.transform.TryGetComponent(out Entity entity)
			&& entity is IDamageable damageable)
			damageable.TakeDamage(Source, Damage);
	}

	private void Update() => OnUpdate();
	public virtual void OnUpdate()
	{
		if (LifeSpan > 0)
			LifeSpan -= Time.deltaTime;
	}

	public void Suicide() => Destroy(gameObject);
}
