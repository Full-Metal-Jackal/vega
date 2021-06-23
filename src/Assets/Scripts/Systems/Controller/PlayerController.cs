using System;
using UnityEngine;

public class PlayerController : MobController
{
	public delegate void PossesAction();

	public event PossesAction OnPossesed;

	/// Да простит меня Аллах
	/// Майки пидоры нельзя несколько базовых классов классов как в плюсах
	private static PlayerController inst;
	public static PlayerController Instance
	{
		get
		{
			if (inst != null)
				return inst;

			Type type = typeof(PlayerController);
			inst = (PlayerController)FindObjectOfType(type);
			if (inst == null)
				Debug.LogWarning($"В сцене нужен экземпляр {type}, но он отсутствует.");

			return inst;
		}
	}
	
	/// <summary>
	/// The entity currently selected by the Possessed.
	/// </summary>
	public IInteractable SelectedEntity { get; protected set; }
	private readonly Collider[] colliderBuffer = new Collider[16];
	private LayerMask interactableMask;

	private bool requestDodging = false;
	private bool isSprinting = false;
	private bool isWalking = false;

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

	private bool usePressed = false;

	private void Awake() =>
		interactableMask = LayerMask.GetMask("Interactables");

	public override bool PossessMob(Mob mob)
	{
		if (!base.PossessMob(mob))
			return false;
		
		if (Game.CameraController)
			Game.CameraController.SetTrackedMob(mob);
		
		OnPossesed?.Invoke();

		return true;
	}
	
	protected override Vector3 GetMovement(out MovementState state)
	{
		state = MovementState.Standing;

		if (!Game.IsWorldInputAllowed)
			return Vector3.zero;

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(x, 0, z);

		// <TODO implement as soon as we switch to the new InputSystem.>
		//isWalking = Input.GetKey("Walk");
		//isSprinting = Input.GetKey("Sprint");

		UpdateActions();
		// The following checks should descend from those of critical importance to the less important ones.
		if (requestDodging)
		{
			state = MovementState.Dodging;
			requestDodging = false;
		}
		else if (isSprinting)
		{
			state = MovementState.Sprinting;
		}
		else if (isWalking)
		{
			state = MovementState.Walking;
		}
		else if (movement.magnitude != 0)
		{
			state = MovementState.Running;
		}

		return movement;
	}

	protected override void OnUpdate(float delta)
	{
		UpdateSelectedEntitiy();
	}

	protected void UpdateActions()
	{
		if (!Game.IsWorldInputAllowed)
			return;

		// We use GetButton instead of GetButtonDown because it will be far simplier
		// to adjust to the new Unity's InputSystem this way.
		if (Input.GetButton("Use"))
		{
			if (!usePressed && SelectedEntity is IInteractable interactable)
				Possessed.Use(interactable);
			usePressed = true;
		}
		else
		{
			usePressed = false;
		}

		if (Input.GetButton("Jump"))
			requestDodging = true;
	}

	public void SetSelectedOutline(bool selected)
	{
		if ((SelectedEntity is Entity entity) && entity.OuterOutline is Outline outline)
			outline.OutlineColor = selected ? selectedColor : deselectedColor;
	}

	public void UpdateSelectedEntitiy()
	{
		IInteractable newSelected = GetSelectedEntity();
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
	/// Won't detect entities with colliders not assigned to the layer from interactableMask.
	/// Will throw an exception if those colliders were applied to something other than entity child.
	/// </summary>
	/// <returns>The selected IInteractable entity.</returns>
	protected virtual IInteractable GetSelectedEntity()
	{
		IInteractable result = null;

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

			if (!(collider.GetComponentInParent<Entity>() is IInteractable interactable)
				|| !interactable.Selectable
				|| !interactable.CanBeUsedBy(Possessed))
				continue;

			minDist = distance;
			result = interactable;
		}

		return result;
	}
}
