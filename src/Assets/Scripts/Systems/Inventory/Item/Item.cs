using UnityEngine;

namespace Inventory
{
	public abstract class Item : MonoBehaviour
	{
		public event System.Action OnSlotTextChanged;

		public bool Initialized { get; private set; } = false;

		[field: SerializeField]
		public ItemData ItemData { get; private set; }

		[field: SerializeField]
		public string Name { get; private set; }

		[field: SerializeField]
		public string Desc { get; private set; }

		[field: SerializeField]
		public float Mass { get; private set; } = 2f;

		[field: SerializeField]
		public SlotType SlotType { get; private set; } = SlotType.Weapon;
		public abstract string SlotText { get; }

		/// <summary>
		/// The model that currently represents this item in the mob's hands.
		/// Should be deleted when the item is not in hands.
		/// </summary>
		public ItemModelData Model { get; protected set; }

		private ItemSlot __slot;

		[field: SerializeField]
		public bool SelectOnPickUp { get; private set; }

		public ItemSlot Slot
		{
			get => __slot;
			set
			{
				if (!(__slot = value))
					return;

				transform.SetParent(__slot.transform);
			}
		}

		/// <summary>
		/// If this item can be fired right now.
		/// </summary>
		public virtual bool CanFire => true;

		/// <summary>
		/// If this item can be reloaded right now.
		/// </summary>
		public virtual bool CanReload => false;

		/// <summary>
		/// How this item should be held in hands. If it shouldn't appear in hands at all, leave it as None.
		/// </summary>
		[field: SerializeField]
		public HoldType HoldType { get; protected set; } = HoldType.None;

		/// <summary>
		/// Shortcut for item slot's owner.
		/// </summary>
		public Mob Owner => (Slot && Slot.Inventory) ? Slot.Inventory.Owner : null;

		/// <summary>
		/// If the owner of this item should aim it at the cursor.
		/// </summary>
		public virtual bool IsAimable => false;

		protected virtual void Awake()
		{
			if (Initialized)
				throw new System.Exception($"Multiple initialization attempts of {this}!");

			Initialized = true;
		}

		/// <summary>
		/// Called when the player tries to use an item, e.g. firing a gun.
		/// </summary>
		/// <param name="target">The target position. Might be unused.</param>
		/// <returns>true if the item has been fired successfully, false otherwise.</returns>
		public virtual bool Fire(Vector3 target)
		{
			if (!CanFire)
				return false;
			return true;
		}

		/// <summary>
		/// Called when a mob tries to reaload the item. Used mostly by guns.
		/// <returns>true if the item has been fired successfully, false otherwise.</returns>
		public virtual bool Reload()
		{
			if (!CanReload)
				return false;
			return true;
		}

		/// <summary>
		/// Called when the item's slot is selected, e.g. when a mob tries to draw a weapon or deploy a drone.
		/// </summary>
		public virtual void Select()
		{
			// <TODO> Dequip mob's active item, then draw this one.
			Equip();
		}

		protected virtual void Equip()
		{
			if (!Owner)
			{
				Debug.LogWarning($"{this} is attempted to be drawn without owner, this should never happen.");
				return;
			}

			if (!(Owner.ItemSocket is Transform socket))
			{
				Debug.LogWarning($"Couldn't find socket for {this} in {Owner}.");
				return;
			}

			if (!(ItemData.PasteModel(socket.transform) is ItemModelData model))
			{
				Debug.LogWarning($"{this} has invalid model setup: no GunModelData detected.");
				return;
			}

			Model = model;
			const float skeletonScale = .01f;  // This shit exists just because it wants to. Don't mess with it.
			Model.transform.localScale = Vector3.one * skeletonScale;
			if (Model.Origin)
			{
				Model.transform.localPosition += Model.Origin.localPosition * skeletonScale;
				Model.transform.localRotation *= Model.Origin.localRotation;
			}

			if (Owner.IsPlayer && ItemData.Cursor)
				Cursor.SetCursor(
					ItemData.Cursor,
					new Vector2(ItemData.Cursor.width / 2, ItemData.Cursor.height / 2),
					CursorMode.Auto
				);

			Owner.ActiveItem = this;
		}

		protected virtual void Unequip()
		{
			if (Owner.ActiveItem != this)
				return;

			if (Model)
				Model.Suicide();

			if (Owner.IsPlayer && ItemData.Cursor)
				Cursor.SetCursor(Game.defaultCursor, Vector2.zero, CursorMode.Auto);

			Owner.ActiveItem = null;
		}

		public bool Drop() => Drop(Vector3.zero);
		public abstract bool Drop(Vector3 force);
		
		protected void UpdateSlotText() =>
			OnSlotTextChanged?.Invoke();
	}

	public abstract class Item<ItemType> : Item where ItemType : Item
	{
		public override bool Drop(Vector3 force)
		{
			if (!Slot)
			{
				Debug.LogWarning($"{Owner}: multiple drop attempts of {this}!");
				return false;
			}

			Unequip();

			Transform orientation = Model ? Model.transform : Owner.transform;
			Pickable<ItemType> dropped = ItemData.PastePickable<ItemType>(orientation.position, orientation.rotation);

			dropped.Setup(this as ItemType);
			dropped.Dynamic.Body.AddForce(force, ForceMode.Impulse);

			Slot.Clear();

			return true;
		}
	}
}
