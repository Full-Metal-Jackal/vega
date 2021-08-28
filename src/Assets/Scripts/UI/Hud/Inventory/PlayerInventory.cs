using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.HUD
{
	public class PlayerInventory : MonoBehaviour
	{
		[SerializeField]
		private ItemSlot slotPrefab;

		[SerializeField]
		private RectTransform slotsHolder;

		private readonly List<ItemSlot> slots = new List<ItemSlot>();

		public void AddItemSlot()
		{
			ItemSlot newSlot = Instantiate(slotPrefab, slotsHolder, false);
			newSlot.transform.SetSiblingIndex(slotsHolder.childCount - 1);
			slots.Add(newSlot);
		}
	}
}
