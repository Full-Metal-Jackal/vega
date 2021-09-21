using UnityEngine;

public class Bullet : Projectile
{
	public override ImpactType ImpactType => ImpactType.Bullet;
	
	public override void Impact(GameObject other)
	{
		base.Impact(other);

		damage.hitPoint = transform.position;
		damage.direction = transform.forward;

		if (
			other.transform.TryGetComponent(out Entity entity) &&
			entity is IDamageable damageable
		)
			damageable.TakeDamage(damage);

		Suicide();
	}
}
