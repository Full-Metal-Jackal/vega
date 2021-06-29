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
		/// The target of the owner's left hand's IK.
		/// </summary>
		[field: SerializeField]
		public Transform LeftHandHandle { get; private set; }

		/// <summary>
		/// The target of the owner's right hand's IK.
		/// </summary>
		[field: SerializeField]
		public Transform RightHandHandle { get; private set; }

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
		public abstract void Use();
	}
}
