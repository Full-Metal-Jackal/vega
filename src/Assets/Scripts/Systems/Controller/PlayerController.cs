using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MobController
{
	public delegate void PossesAction(Mob mob);

	public event PossesAction OnPossesed;

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
	public float selectionDistance = 2f;

	/// <summary>
	/// The color of the outline of selected entities.
	/// </summary>
	public Color selectedColor = Color.white;

	/// <summary>
	/// The color of the outline of deselected entities.
	/// <TODO> implement some constant or something instead of this
	/// </summary>
	public Color deselectedColor = Color.black;

	protected override void Initialize()
	{
		base.Initialize();
		
		interactableMask = LayerMask.GetMask(new string[] { "Interactables", "Items", "Mobs" });
	}

	protected override void Setup()
	{
		base.Setup();

		Actions.World.Use.performed += ctx => OnUsePressed();
		Actions.World.Dodge.performed += ctx => OnDodgePressed();
		Actions.World.Fire.performed += ctx => OnFirePressed();
		Actions.World.Reload.performed += ctx => OnReloadPressed();

		Actions.World.Sprint.performed += ctx => OnSprintInput(true);
		Actions.World.Sprint.canceled += ctx => OnSprintInput(false);

		Actions.World.Move.canceled += ctx => OnMoveInput(ctx.ReadValue<Vector2>());
		Actions.World.Move.performed += ctx => OnMoveInput(ctx.ReadValue<Vector2>());
		Actions.World.Move.started += ctx => OnMoveInput(ctx.ReadValue<Vector2>());
	}

	public override bool PossessMob(Mob mob)
	{
		if (!base.PossessMob(mob))
			return false;
		
		OnPossesed?.Invoke(mob);

		return true;
	}

	protected override Vector3 UpdateMovementInput()
	{
		Vector3 move = Vector3.zero;
		Vector3 movement = new Vector3(move.x, 0, move.y);

		return movement;
	}

	private void OnUsePressed()
	{
		if (SelectedEntity is Interaction interaction)
			Possessed.Use(interaction);
	}

	public void OnFirePressed() => Possessed.Fire();

	public void OnReloadPressed() => Possessed.Reload();

	private void OnDodgePressed() => Possessed.DashAction();

	private void OnSprintInput(bool sprint) =>
		Possessed.MovementType = sprint ? MovementType.Sprinting : MovementType.Running;

	private void OnMoveInput(Vector2 inputMovement) =>
		movement = CameraController.Instance.VerticalRotation * new Vector3(inputMovement.x, 0, inputMovement.y);

	protected override void OnUpdate(float delta)
	{
		UpdateSelectedEntitiy();
		UpdateAimPos();
	}

	public void UpdateAimPos()
	{
		// <TODO> Check if this constant works well enough; otherwise, make it depend on the mob's height/distance to cursor.
		const float aimHeight = 1.5f;

		Vector3 aimPos = CameraController.GetWorldCursorPos();
		aimPos.y += aimHeight;

		Possessed.AimPos = aimPos;
	}

	public void SetSelectedOutline(bool selected)
	{
		if ((SelectedEntity is Interaction interaction) && interaction.Entity.Outline is Outline outline)
			outline.OutlineColor = selected ? selectedColor : deselectedColor;
	}

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
			float distance = Vector3.Distance(mobPos, entityPos);
			if ((Vector3.Angle((entityPos - mobPos), Possessed.transform.forward) > selectionScope)
				|| (distance >= minDist))
				continue;

			if (!(collider.GetComponentInParent<Interaction>() is Interaction interactable)
				|| !interactable.Selectable
				|| !interactable.CanBeUsedBy(Possessed))
				continue;

			minDist = distance;
			result = interactable;
		}

		return result;
	}
}
