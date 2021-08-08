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

		private void Start()
		{
			SetSlotState(false); // initially offset the slot, showing its disabled state

			PlayerController.Instance.Possessed.OnPickedUpItem += PickedUpItemHandler;
			PlayerController.Instance.Possessed.OnDroppedItem += DroppedItemHandler;
		}

		private void OnDestroy()
		{
			PlayerController.Instance.Possessed.OnPickedUpItem -= PickedUpItemHandler;
			PlayerController.Instance.Possessed.OnDroppedItem -= DroppedItemHandler;
		}

		private void PickedUpItemHandler(Item item)
		{
			if (!(item is Gun gun))
				return;

			icon = gun.ItemData.PasteIcon(background.transform, siblingIdx: 0);
			gun.OnAfterFire += () => UpdateAmmoCount(gun);
			this.gun = gun;

			UpdateAmmoCount(gun);

			SetSlotState(true);
		}

		private void DroppedItemHandler()
		{
			SetSlotState(false);
			if (icon)
				Destroy(icon.gameObject);
		}

		private void UpdateAmmoCount(Gun gun) => ammoCount.text = $"{gun.AmmoCount}/{gun.ClipSize}";
		
		private void SetSlotState(bool enabled)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, enabled ? 0 : offset, transform.localPosition.z);
			ammoCount.gameObject.SetActive(enabled);
			this.enabled = enabled;
		}
	}
}
