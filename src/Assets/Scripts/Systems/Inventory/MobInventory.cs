using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
	public class MobInventory : MonoBehaviour
	{
		public bool Initialized { get; private set; } = false;

		public Mob Owner { get; private set; }

		[field: SerializeField]
		public List<ItemSlot> Slots { get; private set; } = new List<ItemSlot>();

		protected int selectedIndex = 0;

		public virtual ItemSlot<ItemType> GetFreeItemSlot<ItemType>() where ItemType : Item
		{
			foreach (ItemSlot slot in Slots)
				if (slot is ItemSlot<ItemType> result && result.IsFree)
				{

					return result;
				}
				else
				{
					print(slot);
				}

			return null;
		}

		private void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize()
		{
			if (Initialized)
				throw new System.Exception($"Multiple initialization attempts of {this}!");

			Owner = transform.parent.GetComponent<Mob>();

			Initialized = true;
		}
	}
}
