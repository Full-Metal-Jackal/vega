using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MobController
{
	public CameraController camController;
	/// <summary>
	/// The entity currently selected by the Possessed.
	/// </summary>
	public IInteractable SelectedEntity { get; protected set; }
	private readonly Collider[] colliderBuffers = new Collider[16];
	private LayerMask interactableMask;

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
		if (!camController && !TryGetComponent(out camController))
			Debug.LogWarning($"Couldn't get camera controller for {this}.");
		interactableMask = LayerMask.GetMask("Interactables");
		return Initialized = base.Initialize();
	}

	public override bool PossessMob(Mob mob)
	{
		bool result = base.PossessMob(mob);
		if (result && camController)
			camController.SetTrackedMob(mob);
		return result;
	}

	protected override Vector3 GetMovement()
	{
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(x, 0, z);
		return movement;
	}

	protected override void OnUpdate()
	{
		UpdateSelectedEntitiy();
		UpdateActions();
	}

	protected void UpdateActions()
	{
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
	}

	public void SetSelectedOutline(bool selected)
	{
		if ((SelectedEntity is Entity entity) && entity.OuterOutline is Outline outline)
			outline.OutlineColor = selected ? selectedColor : deselectedColor;
	}

	void UpdateSelectedEntitiy()
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

		Physics.OverlapSphereNonAlloc(mobPos, selectionDistance, colliderBuffers, interactableMask);
		foreach (Collider collider in colliderBuffers)
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
