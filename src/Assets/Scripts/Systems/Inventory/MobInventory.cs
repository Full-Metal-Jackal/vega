using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
	public class MobInventory : MonoBehaviour
	{
		[field: SerializeField]
		public Mob Owner { get; private set; }

		[field: SerializeField]
		public List<ItemSlot> Slots { get; private set; } = new List<ItemSlot>();

		protected int selectedIndex = 0;

		public virtual ItemSlot<ItemType> GetFreeItemSlot<ItemType>() where ItemType : Item
		{
			foreach (ItemSlot slot in Slots)
				if (slot is ItemSlot<ItemType> result && result.IsFree)
					return result;

			return null;
		}
	}
}
