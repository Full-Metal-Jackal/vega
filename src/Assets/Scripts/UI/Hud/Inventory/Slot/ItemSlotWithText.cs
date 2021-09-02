using UnityEngine;

namespace UI.HUD
{
	public abstract class ItemSlotWithText : ItemSlot
	{
		[SerializeField]
		private TMPro.TextMeshProUGUI text;

		public override Inventory.Item Item
		{
			get => base.Item;
			protected set
			{
				if (Item)
					Item.OnSlotTextChanged -= UpdateText;

				if (base.Item = value)
					value.OnSlotTextChanged += UpdateText;

				UpdateText();
			}
		}

		protected void UpdateText()
		{
			if (text.enabled = Item)
				text.text = Item.SlotText;
		}
	}
}
