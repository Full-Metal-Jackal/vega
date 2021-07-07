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

	[field: SerializeField]
	public ProjecitleType Type { get; private set; } = ProjecitleType.Kinetic;

	private void OnTriggerEnter(Collider other) => OnHit(other);

	public virtual void OnHit(Collider other)
	{
		Suicide();
		if (other.transform.parent.TryGetComponent(out Entity entity)
			&& entity is IDamageable damageable
		)
			damageable.TakeDamage(this, Damage);
	}

	private void Update() => OnUpdate();
	public virtual void OnUpdate()
	{
		if (LifeSpan > 0)
			LifeSpan -= Time.deltaTime;
	}

	public void Suicide() => Destroy(gameObject);
}
