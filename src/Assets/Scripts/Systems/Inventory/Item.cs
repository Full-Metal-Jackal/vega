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
		/// How this item should be held in hands. If it shouldn't appear in hands at all, leave it as None.
		/// </summary>
		[field: SerializeField]
		public HoldType HoldType { get; protected set; } = HoldType.None;

		/// <summary>
		/// Shortcut for item slot's owner.
		/// </summary>
		public Mob Owner => (Slot && Slot.Inventory) ? Slot.Inventory.Owner : null;

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
		/// Called when the item is being used in slot, e.g. when the player tries to draw a weapon or deploy a drone.
		/// </summary>
		public virtual void Use()
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

			if (!(Owner.ItemSocket is ItemSocket socket))
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
			const float skeletonScale = .01f;  // <TODO> Investigate the nature of this scaling later; maybe tweak import settings?
			Model.transform.localScale = Vector3.one * skeletonScale;

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
