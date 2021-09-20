using UnityEngine;

public class GrenadeProjectile : ThrowableProjectile
{
	public override ImpactType ImpactType => ImpactType.Explosion;

	[SerializeField]
	private float explosionRadius = 3f;

	[SerializeField]
	private float detonationTime = 3f;

	private float life;

	protected override void Start()
	{
		life = detonationTime;
	}

	protected override void Update()
	{
		if ((life -= Time.deltaTime) <= 0f)
			Explode();
	}

	protected virtual void Explode()
	{
		Vector3 explosionPos = transform.position;

		foreach (Collider collider in Physics.OverlapSphere(explosionPos, explosionRadius))
		{
			if (collider.transform.parent.TryGetComponent(out Entity entity)
				&& entity is IDamageable damageable)
			{
				Vector3 impactVector = collider.ClosestPoint(explosionPos) - explosionPos;
				float fraction = Mathf.Clamp01(
					1f - (impactVector.magnitude / explosionRadius)
				);

				Damage damage = this.damage;
				damage.amount *= fraction;
				damage.force *= fraction;
				damage.direction = impactVector.normalized;

				damageable.TakeDamage(damage);
			}
			else if (collider.attachedRigidbody)
			{
				collider.attachedRigidbody.AddExplosionForce(damage.force, explosionPos, explosionRadius);
			}
		}

		Suicide();
	}
}
