using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Mob
{
	[field: SerializeField]
	protected Transform AimPosTransform { get; private set; }

	[field: SerializeField]
	public Gun DroneGun { get; protected set; }

	public override Vector3 AimPos
	{
		get => base.AimPos;
		set
		{
			base.AimPos = value;
			AimPosTransform.position = AimPos;
		}
	}

	public override void Move(float delta, Vector3 direction, bool affectY = false)
	{
		base.Move(delta, direction, affectY);

		if (!TurnsToMovementDirection)
			TurnTo(delta, AimDir);
		UpdateAnimations();
	}

	protected virtual void UpdateAnimations()
	{
		Vector3 horDir = transform.forward;
		horDir.y = 0;

		Vector3 movementDir = Quaternion.AngleAxis(
				Vector3.SignedAngle(horDir, activeDirection, Vector3.up),
				Vector3.up
			) * Vector3.forward;
		movementDir *= Body.velocity.magnitude;  // / MoveSpeed;

		Animator.SetFloat("MovementSide", movementDir.x);
		Animator.SetFloat("MovementForward", movementDir.z);
	}

	protected override void Start()
	{
		base.Start();

		PickUpItem(DroneGun);
	}
}
