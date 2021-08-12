using UnityEngine;
using UnityEngine.UI;
using Inventory;

namespace UI
{
	public class WeaponSlot : MonoBehaviour
	{
		[SerializeField]
		private Image background;

		[SerializeField]
		private Text ammoCount;

		private Gun gun;

		private Image icon;

		private const float offset = 48;

		private Mob player;

		private void Start()
		{
			SetSlotState(false); // initially offset the slot, showing its disabled state

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
		}

		private void PickedUpItemHandler(Item item)
		{
			if (!(item is Gun gun))
				return;

			this.gun = gun;
			this.gun.OnAfterFire += UpdateAmmoCount;
			this.gun.OnAfterReloaded += UpdateAmmoCount;
			icon = this.gun.ItemData.PasteIcon(background.transform, siblingIdx: 0);

			UpdateAmmoCount();
			SetSlotState(true);
		}

		private void DroppedItemHandler()
		{
			if (!gun)
				return;
			
			gun.OnAfterFire -= UpdateAmmoCount;
			gun.OnAfterReloaded -= UpdateAmmoCount;

			SetSlotState(false);
			if (icon)
				Destroy(icon.gameObject);

			gun = null;
			icon = null;
		}

		private void UpdateAmmoCount() => ammoCount.text = $"{gun.AmmoCount}/{gun.ClipSize}";
		
		private void SetSlotState(bool enabled)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, enabled ? 0 : offset, transform.localPosition.z);
			ammoCount.gameObject.SetActive(enabled);
			this.enabled = enabled;
		}
	}
}
