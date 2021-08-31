using UnityEngine;

public class Bullet : Projectile
{
	public override void Impact(GameObject other)
	{
		base.Impact(other);

		Suicide();

		if (other.transform.TryGetComponent(out Entity entity)
			&& entity is IDamageable damageable)
			damageable.TakeDamage(Source, damage);
	}
}
