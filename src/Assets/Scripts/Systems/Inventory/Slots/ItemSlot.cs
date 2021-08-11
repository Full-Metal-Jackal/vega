﻿using System;
using UnityEngine;

namespace Inventory
{
	public abstract class ItemSlot : MonoBehaviour
	{
		public bool Initialized { get; private set; } = false;

		public MobInventory Inventory { get; private set; }

		private void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize()
		{
			if (Initialized)
				throw new System.Exception($"Multiple initialization attempts of {this}!");

			Inventory = transform.parent.GetComponent<MobInventory>();

			Initialized = true;
		}

		public virtual bool IsFree => false;

		/// <summary>
		/// Called when the player pushes the slot's button to activate the stored item.
		/// </summary>
		public abstract void OnActivated();

		public abstract void Clear();
	}

	public abstract class ItemSlot<ItemType> : ItemSlot where ItemType : Item
	{
		private ItemType __item;
		public ItemType Item
		{
			get => __item;
			set
			{
				if (!(__item = value))
					return;

				__item.Slot = this;

				// <TODO> Will be removed as soon as we get slot-switching mechanics working.
				__item.Select();
			}
		}

		public override bool IsFree => Item is null;

		public override void OnActivated()
		{
			Item.Select();
		}

		public override void Clear()
		{
			Item.Slot = null;
			Item = null;
		}
	}
}
