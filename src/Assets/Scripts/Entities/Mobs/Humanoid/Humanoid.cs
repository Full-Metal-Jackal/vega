﻿using UnityEngine;

public abstract class Humanoid : Mob
{
	/// <summary>
	/// The multiplier of the mob's walking speed.
	/// </summary>
	public float walkSpeedFactor = .5f;

	/// <summary>
	/// The velocity with which this mob is forced forward when dodgeing.
	/// </summary>
	public float dodgeSpeed = 600f;
	protected float dodgeAngle = 15f;

	/// <summary>
	/// Minimal amount of seconds that should pass betwen two dodge-rolls.
	/// </summary>
	public float dodgeCooldown = 1f;

	/// <summary>
	/// The multiplier of the mob's sprinting speed.
	/// </summary>
	public float sprintSpeedFactor = 1.5f;

	private Vector3 velocityBuffer = Vector3.zero;
	private readonly float movementSmoothing = .01f;

	public bool CanDodge
	{
		get
		{
			if (!CanMoveActively)
				return false;

			switch (MobMovementState)
			{
			case MovementState.Standing:
			case MovementState.Walking:
				return false;
			default:
				break;
			}

			return true;
		}
	}

	public override void Move(
		float delta,
		Vector3 direction,
		MovementState requestedState = MovementState.Running,
		bool affectY = false
	)
	{
		if (!CanMoveActively)
			return;

		if (direction.magnitude > 1f)
			direction.Normalize();
		else if (direction.magnitude <= movementHaltThreshold)
			requestedState = MovementState.Standing;
		activeDirection = direction;

		float speed = moveSpeed;

		switch (requestedState)
		{
		case MovementState.Walking:
			speed *= walkSpeedFactor;
			break;
		case MovementState.Sprinting:
			speed *= sprintSpeedFactor;
			break;
		case MovementState.Dodging:
			if (CanDodge)
			{
				MobMovementState = requestedState;
				return;
			}
			requestedState = MovementState.Running;
			break;
		default:
			break;
		}
		MobMovementState = requestedState;

		Vector3 targetVelocity = speed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;

		Body.velocity = Vector3.SmoothDamp(
			Body.velocity,
			targetVelocity,
			ref velocityBuffer,
			movementSmoothing
		);

		Vector3 rotateTo = Body.velocity;
		rotateTo.y = 0f;
		if (turnsToMovementDirection && rotateTo.magnitude > movementHaltThreshold)
			transform.rotation = Quaternion.LookRotation(rotateTo, Vector3.up);
	}

	public void OnDodgeRoll()
	{
		Vector3 direction = activeDirection.magnitude > 0f ? activeDirection : transform.forward;
		direction.y = 0f;
		direction.Normalize();

		if (turnsToMovementDirection)
			transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

		Vector3 force = Quaternion.AngleAxis(-dodgeAngle, transform.right) * direction * dodgeSpeed;

		Body.velocity = Vector3.zero;
		Body.AddForce(force, ForceMode.Impulse);
	}

	public void OnDodgeRollEnd()
	{
		MobMovementState = MovementState.Sprinting;
	}
}