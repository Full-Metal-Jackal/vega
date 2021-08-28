using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
	public class ItemSlot : MonoBehaviour
	{
		[SerializeField]
		private RectTransform iconHolder;

		private Image icon;

		private Inventory.Item __item;
		public Inventory.Item Item
		{
			get => __item;
			set
			{
				if (__item)
					Destroy(icon.gameObject);

				__item = value;
				icon = __item.ItemData.PasteIcon(iconHolder);
			}
		}
	}
}
