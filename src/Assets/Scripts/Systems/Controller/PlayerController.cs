using System.Linq;
using UnityEngine;

public class PlayerController : MobController
{
	/// <summary>
	/// The interactable entity currently selected by the Possessed.
	/// </summary>
	public Interaction SelectedEntity { get; protected set; }
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

	protected override bool Initialize()
	{
		interactableMask = LayerMask.GetMask("Interactables");

		if (Game.playerController)
			throw new System.Exception($"Multiple instances of camera controller detected: {this}, {Game.playerController}");
		Game.playerController = this;
		
		return Initialized = base.Initialize();
	}

	public override bool PossessMob(Mob mob)
	{
		bool result = base.PossessMob(mob);
		if (result && Game.cameraController)
			Game.cameraController.SetTrackedMob(mob);
		return result;
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
			if (!usePressed && SelectedEntity is Interaction interaction)
				Possessed.Use(interaction);
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
		if ((SelectedEntity is Interaction interaction) && interaction.Entity.OuterOutline is Outline outline)
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
	/// Won't detect entities with colliders not assigned to the layer from interactableMask.
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
