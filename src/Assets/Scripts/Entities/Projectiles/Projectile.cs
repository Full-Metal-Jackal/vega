using System;
using UnityEngine;

public abstract class Projectile : DynamicEntity
{
	public event Action OnImpact;

	protected Damage damage;

	/// <summary>
	/// Who (or what) shot Kennedy.
	/// </summary>
	public Entity Source { get; protected set; }

	public virtual void Setup(Entity source, Damage damage)
	{
		foreach (Collider sourceCollider in source.Colliders)
			foreach (Collider collider in Colliders)
				Physics.IgnoreCollision(collider, sourceCollider);

		Source = source;

		damage.inflictor = source;
		this.damage = damage;
	}

	private void OnCollisionEnter(Collision collision) =>
		Impact(collision.gameObject);

	public virtual void Impact(GameObject other)
	{
		OnImpact?.Invoke();
	}

	public void Suicide() =>
		Destroy(gameObject);
}
