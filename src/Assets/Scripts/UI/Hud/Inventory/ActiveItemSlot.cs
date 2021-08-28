using UnityEngine;
using UnityEngine.UI;
using Inventory;

namespace UI
{
	public class ActiveItemSlot : MonoBehaviour
	{
		[SerializeField]
		private RectTransform iconHolder;

		[SerializeField]
		private TMPro.TextMeshProUGUI text;

		private Item item;

		private Image icon;

		private Mob player;

		private void Start()
		{
			PlayerController.Instance.OnPossessed += (player) =>
			{
				if (this.player)
				{
					this.player.OnPickedUpItem -= PickedUpItemHandler;
					this.player.OnDroppedItem -= DroppedItemHandler;
				}

				this.player = player;
				this.player.OnPickedUpItem += PickedUpItemHandler;
				this.player.OnDroppedItem += DroppedItemHandler;
			};
			UpdateText();
		}

		private void PickedUpItemHandler(Item item)
		{
			this.item = item;
			icon = item.ItemData.PasteIcon(iconHolder);

			item.OnSlotTextChanged += UpdateText;

			UpdateText();
		}

		private void DroppedItemHandler()
		{
			if (!item)
				return;

			if (icon)
			{
				Destroy(icon.gameObject);
				icon = null;
			}

			item.OnSlotTextChanged -= UpdateText;
			item = null;
			
			UpdateText();
		}

		private void UpdateText()
		{
			if (text.enabled = item)
				text.text = item.SlotText;
		}
	}
}
