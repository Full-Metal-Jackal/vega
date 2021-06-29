using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class WeaponSlot : MonoBehaviour
	{
		[SerializeField]
		private RectTransform slotContainer;

		[SerializeField]
		private Image background;

		private Image icon;

		[SerializeField]
		private Text ammoCount;

		private Gun gun;

		private const float offset = 48;

		private void Start()
		{
			enabled = false;
			ammoCount.gameObject.SetActive(false);

			Vector3 origPos = slotContainer.localPosition;
			slotContainer.localPosition = new Vector3(origPos.x, offset, origPos.z); // initially offset the slot, showing its disabled state

			PlayerController.Instance.Possessed.OnPickedUpItem += (item) =>
			{
				if (item is Gun gun)
				{
					icon = gun.ItemData.PasteIcon(background.transform);
					this.gun = gun;

					slotContainer.localPosition = origPos;

					enabled = true;
					ammoCount.gameObject.SetActive(true);
				}
			};
		}

		// <TODO> Rewire to an CSharp event.
		private void Update()
		{
			if (!gun)
				return;

			ammoCount.text = $"{gun.AmmoCount}/{gun.ClipSize}";
		}
	}
}
