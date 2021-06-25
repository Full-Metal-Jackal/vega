using UnityEngine;

using Inventory;

/// <summary>
/// Represents items that can be picked up and stored in a slot.
/// </summary>
[RequireComponent(typeof(DynamicEntity))]
public class Storeable<ItemType> : Interaction where ItemType : Item
{
	// The itemData loaded at start in case this item should be spawned at the game start and create its own item.
	[field: SerializeField]
	private GameObject startItemData;

	public ItemType Item { get; private set; }

	public DynamicEntity Dynamic { get; private set; }

	protected override void Initialize()
	{
		base.Initialize();
		Dynamic = GetComponent<DynamicEntity>();
	}

	private void Start()
	{
		Setup();
	}

	protected virtual void Setup()
	{
		if (!startItemData)
			return;

		if (!startItemData.TryGetComponent(out ItemData itemData))
			throw new System.ArgumentException($"Invalid prefab provided for {this}, the prefab should contain ItemData linking script.");

		if (!itemData.PasteItem(transform).TryGetComponent(out ItemType item))
			throw new System.ArgumentException($"Invalid ItemData prefab provided for {this}.");

		Setup(item);
	}

	protected virtual void Setup(ItemType item)
	{
		item.ItemData.PasteModel(transform);
		item.ItemData.PasteCollisions(transform);

		Item = item;
	}

	// <TODO> there are two methods until we decide whether to separate the item and the visualization parts. 
	public override bool OnUse(Mob mob)
	{
		if (OnPickUp(mob))
		{
			Suicide();
			Debug.Log($"{mob} has picked up {this}.");
			return true;
		}
		else
		{
			return false;
		}
	}

	protected void Suicide() =>
		Destroy(gameObject);

	protected virtual bool OnPickUp(Mob mob)
	{
		ItemSlot<ItemType> slot = mob.Inventory.GetFreeItemSlot<ItemType>();
		
		if (!slot)
			return false;

		slot.Item = Item;

		return true;
	}
}
