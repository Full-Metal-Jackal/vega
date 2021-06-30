using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class WeaponSlot : MonoBehaviour
	{
		[SerializeField]
		private Image background;

		[SerializeField]
		private Text ammoCount;

		private Gun gun;

		private const float offset = 48;

		private void Start()
		{
			enabled = false;
			ammoCount.gameObject.SetActive(false);

			Vector3 origPos = transform.localPosition;
			transform.localPosition = new Vector3(origPos.x, offset, origPos.z); // initially offset the slot, showing its disabled state

			PlayerController.Instance.Possessed.OnPickedUpItem += (item) =>
			{
				if (item is Gun gun)
				{
					gun.ItemData.PasteIcon(background.transform, siblingIdx: 0);
					this.gun = gun;

					transform.localPosition = origPos;

					enabled = true;
					ammoCount.gameObject.SetActive(true);
				}
			};
		}

		// <TODO> Rewire to a CSharp event for the sake of optimization!!
		private void Update()
		{
			if (!gun)
				return;

			ammoCount.text = $"{gun.AmmoCount}/{gun.ClipSize}";
		}
	}
}
