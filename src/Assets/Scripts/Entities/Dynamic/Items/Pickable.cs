using UnityEngine;

using Inventory;

/// <summary>
/// Represents items that can be picked up and stored in a slot.
/// </summary>
[RequireComponent(typeof(DynamicEntity))]
public abstract class Pickable<ItemType> : Interaction where ItemType : Item
{
	// The itemData loaded at start in case this item should be spawned at the game start and create its own item.
	[field: SerializeField]
	private ItemData startItemData;

	public ItemType Item { get; private set; }

	public DynamicEntity Dynamic { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		Dynamic = GetComponent<DynamicEntity>();
	}

	private void Start() => Setup();

	protected virtual void Setup()
	{
		if (!startItemData)
			return;

		if (!startItemData.PasteItem(transform).TryGetComponent(out ItemType item))
			throw new System.ArgumentException($"Invalid ItemData prefab provided for {this}.");

		Setup(item);
	}

	public virtual void Setup(ItemType item)
	{
		item.ItemData.PasteModel(transform);
		item.ItemData.PasteCollisions(transform);

		// <TODO> Update the outline somwhow here.

		Item = item;
	}

	public override bool OnUse(Mob mob)
	{
		if (!OnPickUp(mob))
			return false;

		Suicide();
		
		return true;
	}

	protected void Suicide() => Destroy(gameObject);

	protected virtual bool OnPickUp(Mob mob) => mob.PickUpItem(Item);
}
