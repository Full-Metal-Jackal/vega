using System;
using System.Linq;
using UnityEngine;

public class PlayerController : MobController
{
	public event Action<Mob> OnPossessed;

	/// Да простит меня Аллах
	/// Майки пидоры нельзя несколько базовых классов классов как в плюсах
	private static PlayerController inst;
	public static PlayerController Instance
	{
		get
		{
			if (inst)
				return inst;

			Type type = typeof(PlayerController);
			inst = (PlayerController)FindObjectOfType(type);
			if (!inst)
				Debug.LogWarning($"В сцене нужен экземпляр {type}, но он отсутствует.");

			return inst;
		}
	}

	/// <summary>
	/// The interactable entity currently selected by the Possessed.
	/// </summary>
	public Interaction SelectedEntity { get; protected set; }
	private readonly Collider[] colliderBuffer = new Collider[16];
	private LayerMask interactableMask;

	public Input.InputActions Actions => Input.PlayerInput.Actions;

	/// <summary>
	/// The angular size of selection sector.
	/// </summary>
	public float selectionScope = 45f;

	/// <summary>
	/// The radius of the selection sector.
	/// </summary>
	public float selectionDistance = 1.25f;

	protected override void Awake()
	{
		base.Awake();
		
		interactableMask = LayerMask.GetMask(new string[] { "Interactables", "Items", "Mobs" });
	}

	protected override void Start()
	{
		base.Start();

		Actions.World.Use.performed += ctx => InputUse();
		Actions.World.Dodge.performed += ctx => InputDodge();
		Actions.World.Reload.performed += ctx => InputReload();
		Actions.World.Throw.performed += ctx => InputThrow();
		Actions.World.SpecialAbilty.performed += ctx => InputSpecialAbility();
		Actions.World.Drop.performed += ctx => InputDrop();

		Actions.World.Fire.performed += ctx => InputTrigger(true);
		Actions.World.Fire.canceled += ctx => InputTrigger(false);

		// Actions.World.Sprint.performed += ctx => OnSprintInput(true);
		// Actions.World.Sprint.canceled += ctx => OnSprintInput(false);

		Actions.World.Move.canceled += ctx => InputMove2D(ctx.ReadValue<Vector2>());
		Actions.World.Move.performed += ctx => InputMove2D(ctx.ReadValue<Vector2>());
		Actions.World.Move.started += ctx => InputMove2D(ctx.ReadValue<Vector2>());
	}

	public override void PossessMob(Mob mob)
	{
		base.PossessMob(mob);

		CameraController.Instance.SetTrackedMob(mob);
		CameraController.Instance.followingCursor = true;

		OnPossessed?.Invoke(mob);
	}

	protected override void Update()
	{
		if (!Possessed)
			return;

		UpdateSelectedEntitiy();
		UpdateAimPos();
	}

	public void UpdateAimPos()
	{
		float cursorHeight;
		if (Possessed.ActiveItem != null && Possessed.ActiveItem.IsAimable)
			cursorHeight = Possessed.ActiveItem.AimOrigin.y;
		else
			cursorHeight = Possessed.transform.position.y + Possessed.AimHeight;

		Possessed.AimPos = CameraController.GetWorldCursorPos(heightOffset: cursorHeight);
	}
	protected override void InputUse()
	{
		if (SelectedEntity is Interaction interaction)
			Possessed.Use(interaction);
	}

	public void SetSelectedOutline(bool selected)
	{
		if (SelectedEntity)
			SelectedEntity.OutlineEnabled = selected;
	}

	// Overloading the method makes it recursive, hence the different name.
	private void InputMove2D(Vector2 inputMovement) =>
		InputMove(
			CameraController.Instance.VerticalRotation *
			new Vector3(inputMovement.x, 0, inputMovement.y)
		);

	public void UpdateSelectedEntitiy()
	{
		Interaction newSelected = GetSelectedEntity();
		if (newSelected != SelectedEntity)
		{
			SetSelectedOutline(false);
			SelectedEntity = newSelected;
			SetSelectedOutline(true);
		}
	}

	/// <summary>
	/// Gets the entity the possessed mob is currently selecting.
	/// Selecting means that the entity is within sector with radius equal to selectionDistance and angle equal to double selectionScope.
	/// 
	/// Won't detect entities with colliders not assigned to a layer from interactableMask.
	/// Will throw an exception if those colliders were applied to something other than entity child.
	/// </summary>
	/// <returns>The selected entity's Interaction component.</returns>
	protected virtual Interaction GetSelectedEntity()
	{
		Interaction result = null;

		Vector3 mobPos = Possessed.transform.position;
		float minDist = selectionDistance;

		Physics.OverlapSphereNonAlloc(mobPos, selectionDistance, colliderBuffer, interactableMask);
		foreach (Collider collider in colliderBuffer)
		{
			if (!collider)
				continue;

			Vector3 entityPos = collider.ClosestPoint(mobPos);
			float distance = Utils.HorizontalDistance(mobPos, entityPos);
			if ((Vector3.Angle((entityPos - mobPos), Possessed.transform.forward) > selectionScope)
				|| (distance >= minDist))
				continue;

			Interaction interaction = collider.GetComponentsInParent<Interaction>().FirstOrDefault(
				(Interaction i) => i.Selectable && i.CanBeUsedBy(Possessed)
			);
			if (interaction == null)
				continue;

			minDist = distance;
			result = interaction;
		}

		return result;
	}
}
