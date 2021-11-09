using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Mob
{
	[field: SerializeField]
	private List<Transform> Aimables = new List<Transform>();

	[field: SerializeField]
	public Gun DroneGun { get; protected set; }

	public override Vector3 AimPos
	{
		get => base.AimPos;
		set
		{
			base.AimPos = value;
			
			foreach (Transform aimable in Aimables)
			{
				aimable.up = AimPos - aimable.position;
			}
		}
	}

	public override void Move(float delta, Vector3 direction, bool affectY = false)
	{
		base.Move(delta, direction, affectY);

		TurnTo(delta, AimDir);
	}

	protected override void Start()
	{
		base.Start();

		print($"picked up {PickUpItem(DroneGun)}, {ActiveItem}");
	}
}
