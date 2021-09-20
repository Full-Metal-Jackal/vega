using System;
using UnityEngine;

namespace Inventory
{
	public class ItemSlot : MonoBehaviour
	{
		[field: SerializeField]
		public SlotType Type { get; private set; }

		private Item __item;
		public Item Item
		{
			get => __item;
			set
			{
				if (!(__item = value))
					return;

				__item.Slot = this;

				if (__item.SelectOnPickUp)
					__item.Select();
			}
		}

		public MobInventory Inventory { get; private set; }

		protected virtual void Awake()
		{
			Inventory = transform.parent.GetComponent<MobInventory>();
		}

		public bool IsFree => Item is null;

		/// <summary>
		/// Called when the player pushes the slot's button to activate the stored item.
		/// </summary>
		public void OnActivated()
		{
			Item.Select();
		}

		public void Clear()
		{
			Item.Slot = null;
			Item = null;
		}
	}
}
