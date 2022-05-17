using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is supposed to be used by drones, turrets and such.
// Basically, it's a simple version of gun without a model of its own.
public class IntegratedGun : Gun
{
	[field: SerializeField]
	public Transform[] Barrels { get; private set; }
	private int currentBarrel = 0;
	public override Transform Barrel => Barrels[currentBarrel % Barrels.Length];

	public override bool ConsumeAmmo() => true;

	public override void PostFire(Vector3 direction, Projectile projectile)
	{
		base.PostFire(direction, projectile);
		currentBarrel++;
	}
}
