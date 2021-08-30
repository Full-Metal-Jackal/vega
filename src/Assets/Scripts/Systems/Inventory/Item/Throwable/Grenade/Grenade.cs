using UnityEngine;

public class Grenade : Throwable
{
	public float DetonationTime { get; protected set; }

	private float timeUntilDetonation = 0f;

	public override bool Fire(Vector3 target)
	{
		if (!base.Fire(target))
			return false;

		timeUntilDetonation = DetonationTime;
		
		return true;
	}

	protected void Update()
	{
		if ((timeUntilDetonation -= Time.deltaTime) > 0f)
			return;

		Explode();
	}

	protected virtual void Explode()
	{
		print($"Let's assume {this} has exploded.");
		Destroy(gameObject);
	}
}
