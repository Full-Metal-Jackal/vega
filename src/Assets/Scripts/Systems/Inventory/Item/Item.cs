using UnityEngine;

namespace Inventory
{
	/// <summary>
	/// Represents everything that can be stored in a mob's item slot: guns, drones, devices, may be grenades?
	/// </summary>
	public abstract class Item : MonoBehaviour
	{
		public bool Initialized { get; private set; } = false;

		[field: SerializeField]
		public ItemData ItemData { get; private set; }

		[field: SerializeField]
		public string Name { get; private set; }

		[field: SerializeField]
		public string Desc { get; private set; }

		/// <summary>
		/// The model that currently represents this item in the mob's hands.
		/// Should be deleted when the item is not in hands.
		/// </summary>
		public ItemModelData Model { get; protected set; }

		private ItemSlot slot;
		public ItemSlot Slot
		{
			get => slot;
			set
			{
				slot = value;
				transform.SetParent(slot.transform);
			}
		}

		/// <summary>
		/// If this item can be fired right now.
		/// </summary>
		public virtual bool CanFire => true;

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

		private void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize()
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
		public virtual bool TryFire(Vector3 target)
		{
			if (!CanFire)
				return false;
			Fire(target);
			return true;
		}

		protected virtual void Fire(Vector3 target)
		{
		}

		/// <summary>
		/// Called when the item's slot is selected, e.g. when the player tries to draw a weapon or deploy a drone.
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

			if (ItemData.Cursor)
				Cursor.SetCursor(
					ItemData.Cursor,
					new Vector2(ItemData.Cursor.width / 2, ItemData.Cursor.height / 2),
					CursorMode.Auto
				);

			Owner.ActiveItem = this;
		}

		protected virtual void Unequip()
		{
			Model.Suicide();
			Owner.ActiveItem = null;
		}

		public void Drop()
		{
			if (!Slot)
			{
				Debug.LogWarning($"Multiple drop attempts of {this}!");
				return;
			}

			// <TODO> Spawn an item prefab at the mob's position and call its Setup with this item.
		}
	}
}
