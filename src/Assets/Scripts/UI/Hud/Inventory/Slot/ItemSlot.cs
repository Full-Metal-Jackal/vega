using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
	public class ItemSlot : MonoBehaviour
	{
		[field: SerializeField]
		protected RectTransform IconHolder { get; private set; }
		public virtual Mob Player { get; protected set; }

		private Image icon;

		private Inventory.Item __item;
		public virtual Inventory.Item Item
		{
			get => __item;
			protected set
			{
				if (__item)
					Destroy(icon.gameObject);

				if (__item = value)
					icon = __item.ItemData.PasteIcon(IconHolder);
			}
		}

		private void Start()
		{
			PlayerController.Instance.OnPossessed += (player) => Player = player;
		}

		public virtual void SetItem(Inventory.Item item) =>
			Item = item;
	}
}
