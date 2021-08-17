using UnityEngine;
using System;

using Inventory;

using static Utils;

public abstract class Mob : DynamicEntity, IDamageable
{
	public event Action OnItemChanged;
	public event Action<Item> OnPickedUpItem;
	public event Action OnDroppedItem;
	public event Action OnHealthChanged;
	public event Action<Mob> OnDefeated;

	[field: SerializeField]
	public virtual float MaxHealth { get; set; } = 100;

	[SerializeField, EditorEx.Prop(ReadOnly = true, Name = "Health")]
	private float __health;
	public float Health
	{
		get => __health;
		protected set
		{
			__health = value;
			OnHealthChanged?.Invoke();
		}
	}

	public float CriticalHealth { get; protected set; }

	[SerializeField, EditorEx.Prop(ReadOnly = true)]
	private float __stamina;
	public float Stamina
	{
		get => __stamina;
		protected set
		{
			if (value < __stamina)
				lastStaminaDrain = Time.time;
			
			__stamina = Mathf.Clamp(value, 0, MaxStamina);
		}
	}

	[field: SerializeField]
	public virtual float MaxStamina { get; set; } = 100;
	
	/// <summary>
	/// Stamina regeneration speed, measured in points per second.
	/// </summary>
	[field: SerializeField]
	public float StaminaRegenSpeed { get; protected set; } = 20f;
	protected bool CanRegenStamina
	{
		get
		{
			if (MovementType == MovementType.Sprinting)
				return false;

			switch (State)
			{
			case MobState.Dead:
			case MobState.Dodging:
				return false;
			}

			return true;
		}
	}

	/// <summary>
	/// How much seconds should pass before the stamina begins to regenerate.
	/// </summary>
	[field: SerializeField]
	public float StaminaRegenDelay { get; protected set; } = .75f;

	protected float lastStaminaDrain;

	[field: SerializeField]
	protected Animator Animator { get; private set; }

	public MobInventory Inventory { get; private set; }
	public float ItemDropSpeed { get; private set; } = 2f;

	public virtual Transform ItemSocket => transform;

	public bool IsPlayer => PlayerController.Instance.Possessed == this;

	/// <summary>
	/// The mob's running speed.
	/// </summary>
	[field: SerializeField]
	public float MoveSpeed { get; private set; } = 250f;

	public virtual Vector3 AimPos { get; set; }

	public Vector3 AimDir => AimPos - transform.position;
	public float AimDistance => HorizontalDistance(transform.position, AimPos);


	protected readonly float movementHaltThreshold = .01f;

	[SerializeField]
	private readonly bool turnsToMovementDirection = true;
	protected readonly float rotationThreshold = .01f;

	protected Vector3 activeDirection = Vector3.zero;

	[field: SerializeField]
	protected float MaxTurningSpeed { get; private set; } = 10f;

	[field: SerializeField]
	public float AimHeight { get; private set; } = 1.7f;

	public bool Alive { get; protected set; } = true;

	public MobController Controller { get; set; }

	public virtual bool CanUseItems
	{
		get
		{
			// Resembles CanMoveActively pretty much right now but may differ from it later.
			switch (State)
			{
			case MobState.Dead:
			case MobState.Dodging:
			case MobState.Unconscious:
				return false;
			}

			return true;
		}
	}
	public virtual bool CanFire => CanUseItems;
	public virtual bool CanReload => CanUseItems;
	public virtual bool CanDropItems => CanUseItems;

	private Item __activeItem;
	public virtual Item ActiveItem
	{
		get => __activeItem;
		set
		{
			__activeItem = value;
			OnItemChanged?.Invoke();
		}
	}

	private MobState __state = MobState.Standing;
	/// <summary>
	/// The current state of the mob, represents mostly the animation that is being played right now.
	/// </summary>
	public virtual MobState State
	{
		get => __state;
		protected set
		{
			__state = value;
			if (!Animator)
				return;

			const string animatorVariable = "MovementState";
			switch (__state)
			{
			case MobState.Standing:
				Animator.SetInteger(animatorVariable, 0);
				break;
			case MobState.Walking:
				Animator.SetInteger(animatorVariable, 1);
				break;
			case MobState.Running:
				Animator.SetInteger(animatorVariable, 2);
				break;
			case MobState.Sprinting:
				Animator.SetInteger(animatorVariable, 3);
				break;
			case MobState.Dodging:
				Animator.SetTrigger("DodgeRollTrigger");
				break;
			}
		}
	}

	/// <summary>
	/// The way this mob moves continuously: walking, running or sprinting.
	/// </summary>
	public virtual MovementType MovementType { get; set; } = MovementType.Running;

	protected override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		Health = MaxHealth;
		Stamina = MaxStamina;

		Inventory = GetComponentInChildren<Inventory.MobInventory>();

		return true;
	}

	public virtual void TakeDamage(Entity inflictor, float damage)
	{
		Debug.Log($"{this} took {damage} damage from {inflictor}.");
		
		if ((Health -= damage) < 0f)
			Die();
	}

	/// <summary>
	/// Attempts to make the mob dash, dodgeroll etc.
	/// </summary>
	public virtual void DashAction()
	{
	}

	/// <summary>
	/// Handles the mob's active movement.
	/// </summary>
	/// <param name="delta">Delta between two frames.</param>
	/// <param name="direction">Vector describing the movement direction of the mob.
	/// Pass vector with near zero magnitude to make the mob stand.</param>
	/// <param name="affectY">If the request should influence the mob's vertical movement.</param>
	public virtual void Move(
		float delta,
		Vector3 direction,
		bool affectY = false
	)
	{
		if (!CanMoveActively)
			return;

		if (direction.magnitude <= movementHaltThreshold)
		{
			State = MobState.Standing;
		}
		else
		{
			if (direction.magnitude > 1f)
				direction.Normalize();
			State = MobState.Running;
		}

		activeDirection = direction;

		Vector3 targetVelocity = MoveSpeed * direction * delta;
		if (!affectY)
			targetVelocity.y = Body.velocity.y;

		Body.velocity = targetVelocity;

		if (turnsToMovementDirection)
			TurnTo(delta, Body.velocity);
	}

	/// <summary>
	/// Instantly rotates the mob's whole body to face the specified direction.
	/// </summary>
	/// <param name="rotateTo">The direction to face.</param>
	public virtual void SnapTurnTo(Vector3 rotateTo)
	{
		rotateTo.y = 0f;
		if (rotateTo.magnitude <= rotationThreshold)
			return;

		transform.rotation = Quaternion.LookRotation(rotateTo, Vector3.up);
	}

	/// <summary>
	/// continuosly rotates the mob's whole body to face the specified direction.
	/// Should be continuosly called within Update or FixedUpdate calls to work properly.
	/// </summary>
	/// <param name="delta">Delta between two frames.</param>
	/// <param name="rotateTo">The direction to face.</param>
	public virtual void TurnTo(float delta, Vector3 rotateTo)
	{
		rotateTo.y = 0f;
		if (rotateTo.magnitude <= rotationThreshold)
			return;

		const float maxAngle = 180f;
		float differenceFactor = Mathf.Lerp(.5f, 1.5f,
			Vector3.Angle(transform.forward, rotateTo) / maxAngle
			);
		transform.rotation = Quaternion.LookRotation(
			Vector3.RotateTowards(
				transform.forward,
				rotateTo.normalized,
				differenceFactor * MaxTurningSpeed * delta,
				0
			),
			Vector3.up
		);
	}

	/// <summary>
	/// Makes the mob possessed by the provided controller.
	/// </summary>
	/// <param name="controller">The controller that should possess the mob.</param>
	/// <returns>true if the mob has been possessed succesfully, false otherwise.</returns>
	public bool SetPossessed(MobController controller)
	{
		if (Controller)
			return false;
		Controller = controller;
		return true;
	}

	public virtual bool PickUpItem<ItemType>(ItemType item) where ItemType : Item
	{
		ItemSlot<ItemType> slot = Inventory.GetFreeItemSlot<ItemType>();
		if (!slot)
			return false;

		slot.Item = item;
		OnPickedUpItem?.Invoke(item);

		return true;
	}

	public virtual bool Use(Interaction interaction) => interaction && interaction.CanBeUsedBy(this) && interaction.OnUse(this);

	public bool CanMoveActively
	{
		get
		{
			switch (State)
			{
			case MobState.Dead:
			case MobState.Dodging:
			case MobState.Unconscious:
				return false;
			}

			return true;
		}
	}

	public virtual void Fire()
	{
		if (ActiveItem && CanFire)
			ActiveItem.Fire(AimPos);
	}

	public virtual void Reload()
	{
		if (ActiveItem && CanReload)
			ActiveItem.Reload();
	}

	public virtual void DropItem() => DropItem(ActiveItem);

	public virtual void DropItem(Item item)
	{
		if (item is null)
			return;

		if (!CanDropItems)
			return;

		if (item.Owner != this)
			return;

		item.Drop(transform.forward * ItemDropSpeed + Body.velocity);
		OnDroppedItem?.Invoke();
	}

	protected override void Tick(float delta)
	{
		base.Tick(delta);
		UpdateStaminaRegeneration(delta);
	}

	protected virtual void UpdateStaminaRegeneration(float delta)
	{
		if (!CanRegenStamina)
			return;

		if (lastStaminaDrain + StaminaRegenDelay > Time.time)
			return;

		Stamina += StaminaRegenSpeed * delta;
	}

	public void Die()
	{
		Debug.Log($"{this} died.");
		State = MobState.Dead;

		OnDefeated?.Invoke(this);
	}
}
