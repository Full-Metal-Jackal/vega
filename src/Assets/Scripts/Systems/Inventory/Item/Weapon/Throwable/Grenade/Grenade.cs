using UnityEngine;

public class Grenade : Throwable
{
	public float DetonationTime { get; protected set; }

	public override bool Fire(Vector3 target)
	{
		if (!base.Fire(target))
			return false;

		return true;
	}

	public override Projectile CreateProjectile()
	{
		Projectile projectile = base.CreateProjectile();
		return projectile;
	}
}
