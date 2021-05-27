using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Entity, IDamageable, IPossessable
{
	public virtual float MaxHealth { get; set; } = 100;
	public float Health { get; protected set; }
	/// <summary>
	/// The mob's runnning speed.
	/// </summary>
	public float MoveSpeed { get; set; } = 400f;
	public float walkSpeedFactor = .5f;
	public float sprintSpeedFactor = 1.5f;
	public readonly float movementHaltThreshold = .01f;
	public readonly bool turnsToMovementDirection = true;

	public bool Alive { get; protected set; } = true;

	public float MovementSmoothing { get; protected set; } = .0f;

	public MobController Controller { get; set; }

	private MovementState movementState;
	public MovementState MobMovementState
	{
		get => movementState;
		set
		{
			movementState = value;
			if (!animator)
				return;
			string animatorVariable = "MovementState";
			switch (movementState)
			{
				case MovementState.Standing:
					animator.SetInteger(animatorVariable, 0);
					break;
				case MovementState.Walking:
					animator.SetInteger(animatorVariable, 1);
					break;
				case MovementState.Running:
					animator.SetInteger(animatorVariable, 2);
					break;
				case MovementState.Sprinting:
					animator.SetInteger(animatorVariable, 3);
					break;
				default:
					break;
			}
		}
	}

	protected Animator animator;

	public Mob()
	{
		Name = "unnamed mob";
		Health = MaxHealth;
	}

	protected override bool Initialize()
	{
		try
		{
			animator = GetComponentInChildren<Animator>();
		}
		catch (ArgumentException)
		{
			Debug.LogError($"Couldn't get animator component for {this}!");
			return false;
		}

		return base.Initialize();
	}

	public void TakeDamage()
	{
		throw new System.NotImplementedException();
	}

	protected override void Tick(float delta)
	{
	}

	/// <summary>
	/// Handles the mob's active movement
	/// </summary>
	/// <param name="delta">delta between two ticks</param>
	/// <param name="movement">vector describing the movement of the mob with magnitude between 0 and 1</param>
	/// <param name="targetState">the movement state that is trying to be enforced on the mob</param>
	public virtual void Move(
		float delta, Vector3 movement,
		MovementState targetState = MovementState.Running
		)
	{
		if (!Initialized)
			return;

		if (movement.magnitude > 1f)
			movement.Normalize();

		if (movement.magnitude <= movementHaltThreshold)
		{
			Body.velocity = Vector3.zero;
			MobMovementState = MovementState.Standing;
			return;
		}

		// <TODO> That'll do for now, but we should implement this shit as soon as we get dodgerolls and lying/dead states.
		switch (targetState)
		{
			default:
				break;
		}

		MobMovementState = targetState;

		Vector3 targetVelocity = MoveSpeed * movement * delta;

		Vector3 currentVelocity = Vector3.zero;
		Body.velocity = Vector3.SmoothDamp(
			Body.velocity,
			targetVelocity,
			ref currentVelocity,
			MovementSmoothing
		);
		if (turnsToMovementDirection)
			transform.rotation = Quaternion.LookRotation(Body.velocity, Vector3.up);
	}

	public bool SetPossessed(MobController controller)
	{
		if (Controller)
			return false;
		Controller = controller;
		return true;
	}
}
