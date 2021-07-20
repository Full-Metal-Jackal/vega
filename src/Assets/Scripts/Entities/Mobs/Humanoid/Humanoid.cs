using UnityEngine;

using Inventory;

public abstract class Humanoid : Mob
{
	public delegate void GunPutDownAction(bool isgunPutDown);

	/// <summary>
	/// The multiplier of the mob's walking speed.
	/// </summary>
	[field: SerializeField]
	public float WalkSpeedFactor { get; private set; } = .5f;

	/// <summary>
	/// The velocity with which this mob is forced forward when dodgeing.
	/// </summary>
	public float DodgeSpeed { get; private set; } = 600f;
	protected float dodgeAngle = 10f;

	/// <summary>
	/// Minimal amount of seconds that should pass betwen two dodge-rolls.
	/// </summary>
	[field: SerializeField]
	public float DodgeCooldown { get; private set; } = 1f;

	/// <summary>
	/// How much stamina does a dodgeroll withdraw.
	/// </summary>
	[field: SerializeField]
	public float DodgeStaminaCost { get; private set; } = 25f;

	/// <summary>
	/// How much stamina does sprinting withdraw every frame.
	/// </summary>
	[field: SerializeField]
	public float SprintStaminaCost { get; private set; } = 15f;

	/// <summary>
	/// The multiplier of the mob's sprinting speed.
	/// </summary>
	[field: SerializeField]
	public float SprintSpeedFactor { get; private set; } = 1.5f;

	private Vector3 velocityBuffer = Vector3.zero;
	private readonly float movementSmoothing = .01f;

	[SerializeField]
	private Transform rightHandSocket;
	public override Transform ItemSocket => rightHandSocket;

	[SerializeField]
	private float aimPosSmoothing = .2f;
	private Vector3 aimPosSmoothingVelocity = Vector3.zero;
	public Vector3 SmoothedAimPos { get; protected set; }
	public void UpdateSmoothedAimPos() => SmoothedAimPos = Vector3.SmoothDamp(
			SmoothedAimPos,
			AimPos,
			ref aimPosSmoothingVelocity,
			aimPosSmoothing
		);

	private bool isAiming = false;
	/// <summary>
	/// Is the mob should currently aim, used for animation.
	/// </summary>
	public bool IsAiming
	{
		get => isAiming;
		protected set => Animator.SetBool("IsAiming", isAiming = value);
	}

	private bool isReloading = false;
	/// <summary>
	/// Is the mob is currently reloading, used for animation.
	/// </summary>
	public bool IsReloading
	{
		get => isReloading;
		protected set
		{
			if (isReloading = value)
				Animator.SetTrigger("ReloadTrigger");
		}
	}

	public override bool CanFire => base.CanFire && IsAiming;

	public override bool CanReload => base.CanReload && !IsReloading;

	/// <summary>
	/// The minimum AimDistance required to aim.
	/// </summary>
	public virtual float MinAimDistance => 1f;
	/// <summary>
	/// Additive distance for MinAimDistance to start aiming.
	/// </summary>
	public virtual float AimEnableDistance => 1f;

	private HoldType holdState = HoldType.None;
	/// <summary>
	/// Represents the aiming animation that is being played right now.
	/// </summary>
	public virtual HoldType HoldState
	{
		get => holdState;
		protected set
		{
			holdState = value;
			if (!Animator)
				return;

			int animatorValue = 0;
			switch (holdState)
			{
			case HoldType.SingleHandPistol:
				animatorValue = 1;
				break;
			case HoldType.TwoHandsPistol:
				animatorValue = 2;
				break;
			case HoldType.AssaultRifle:
				animatorValue = 3;
				break;
			case HoldType.Shotgun:
				animatorValue = 4;
				break;

			}
			Animator.SetInteger("HoldType", animatorValue);
		}
	}

	public bool CanDodge
	{
		get
		{
			if (!CanMoveActively)
				return false;

			switch (State) 
			{
			case MobState.Standing:
			case MobState.Walking:
				return false;
			}

			return Stamina > DodgeStaminaCost;
		}
	}

	public bool CanSprint
	{
		get
		{
			if (!CanMoveActively)
				return false;

			if (State == MobState.Standing)
				return false;

			return Stamina > SprintStaminaCost;
		}
	}

	public override MovementType MovementType
	{
		get => base.MovementType;
		set
		{
			if (value == MovementType.Sprinting && !CanSprint)
				return;
			base.MovementType = value;
		}
	}

	public override Item ActiveItem
	{
		get => base.ActiveItem;
		set
		{
			HoldState = (base.ActiveItem = value) ? value.HoldType : HoldType.None;
			if (!HasAimableItem)
				ResetLegsAnimation();
		}
	}
	public bool HasAimableItem => ActiveItem && ActiveItem.IsAimable;

	public override void Move(float delta, Vector3 direction, bool affectY = false)
	{
		if (!CanMoveActively)
			return;

		float speed = MoveSpeed;

		if (direction.magnitude <= movementHaltThreshold)
		{
			State = MobState.Standing;
		}
		else
		{
			if (direction.magnitude > 1f)
				direction.Normalize();

			switch (MovementType)
			{
			case MovementType.Walking:
				speed *= WalkSpeedFactor;
				State = MobState.Walking;
				break;
			case MovementType.Sprinting:
				if (!CanSprint)
					goto default;

				speed *= SprintSpeedFactor;
				Stamina -= SprintStaminaCost * delta;
				State = MobState.Sprinting;
				break;
			default:
				State = MobState.Running;
				break;
			}
		}

		activeDirection = direction;
		if (HasAimableItem)
		{
			UpdateLegsAnimation();
			UpdateAiming(delta);
		}
		else
		{
			TurnTo(delta, direction);
		}

		Vector3 targetVelocity = speed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;

		Body.velocity = Vector3.SmoothDamp(
			Body.velocity,
			targetVelocity,
			ref velocityBuffer,
			movementSmoothing
		);
	}

	/// <summary>
	/// Tells the legs whether they should run normally, strafe or run backwards.
	/// </summary>
	protected virtual void UpdateLegsAnimation()
	{
		Vector3 horDir = transform.forward;
		horDir.y = 0;

		Vector3 relativeMovDir = Quaternion.AngleAxis(
			Vector3.SignedAngle(horDir, activeDirection, Vector3.up),
			Vector3.up
		) * Vector3.forward;

		Animator.SetFloat("MovementSide", relativeMovDir.x);
		Animator.SetFloat("MovementForward", relativeMovDir.z);
	}

	protected virtual void ResetLegsAnimation()
	{
		Animator.SetFloat("MovementSide", 0f);
		Animator.SetFloat("MovementForward", 1f);
	}

	/// <summary>
	/// Updates the mob's aiming status and rotation.
	/// </summary>
	/// <param name="delta"></param>
	protected virtual void UpdateAiming(float delta)
	{
		if (IsAiming &= AimDistance >= MinAimDistance)
			TurnTo(delta, SmoothedAimPos - transform.position);
		else
			IsAiming = AimDistance >= AimEnableDistance + MinAimDistance;
	}

	public override void DashAction() => DodgeRoll();

	/// <summary>
	/// Performs a dodge roll attempt.
	/// </summary>
	public void DodgeRoll()
	{
		if (CanDodge)
			State = MobState.Dodging;
	}

	public void OnDodgeRoll()
	{
		Vector3 direction = activeDirection.magnitude > 0f ? activeDirection : transform.forward;
		direction.y = 0f;
		direction.Normalize();

		SnapTurnTo(direction);

		Vector3 force = Quaternion.AngleAxis(-dodgeAngle, transform.right) * direction * DodgeSpeed;

		Stamina -= DodgeStaminaCost;

		Body.velocity = Vector3.zero;
		Body.AddForce(force, ForceMode.Impulse);
	}

	public void OnDodgeRollEnd() =>
		State = MobState.Sprinting;

	protected override void Tick(float delta)
	{
		base.Tick(delta);

		UpdateSmoothedAimPos();
	}

	public override void Reload() =>
		IsReloading = ActiveItem && ActiveItem.CanReload && CanReload;

	public void OnReloadEnd()
	{
		ActiveItem.Reload();
		IsReloading = false;
	}
}
