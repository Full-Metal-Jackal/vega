using System;
using Inventory;
using UnityEngine;

public abstract class Weapon : Item<Weapon>
{
	[field: SerializeField]
	public Damage Damage { get; private set; } = new Damage(10f);
}
