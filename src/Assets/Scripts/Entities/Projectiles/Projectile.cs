using UnityEngine;

public class Projectile : DynamicEntity
{
	public float damage = 10f;

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
			damageable.TakeDamage(Source, damage);
	}

	public void Suicide() => Destroy(gameObject);
}
