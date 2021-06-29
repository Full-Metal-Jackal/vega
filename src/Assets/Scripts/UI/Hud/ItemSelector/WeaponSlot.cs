using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
	[SerializeField]
	private GameObject slotContainer;

	private RectTransform slotTransform;
	private Image slotImage;
	private Text ammoCount;
	private Gun gun;

	private const float offset = 48;

	private void Awake() => Initialize();

	private void Start()
	{
		enabled = false;
		ammoCount.gameObject.SetActive(false);

		Vector3 origPos = slotTransform.localPosition;
		slotTransform.localPosition = new Vector3(origPos.x, offset, origPos.z); // initially offset the slot, showing its disabled state

		PlayerController.Instance.Possessed.OnPickedUpItem += (item) =>
		{
			if (item is Gun gun)
			{
				GameObject iconContainer = gun.ItemData.Icon;
				Image gunImg = iconContainer.GetComponentInChildren<Image>();
				if (!gunImg)
					throw new Exception($"Cannot find {gunImg.GetType()} component in {iconContainer} or its children");

				this.gun = gun;

				slotImage.sprite = gunImg.sprite;
				slotImage.color = Color.white;
				slotTransform.localPosition = origPos;

				enabled = true;
				ammoCount.gameObject.SetActive(true);
			}
		};
	}

	private void Initialize()
	{
		Image[] images = slotContainer.GetComponentsInChildren<Image>();
		if (images.Length != 2 || !images[1]?.transform.parent)
			throw new Exception($"{slotContainer} must have an {typeof(Image)} component background with a child {typeof(Image)} component");

		// If the need arises (e.g. to change the background color), images[0] should be the background image
		slotImage = images[1];
		ammoCount = slotContainer.GetComponentInChildren<Text>();
		slotTransform = slotContainer.GetComponent<RectTransform>();
	}

	private void Update() =>
		ammoCount.text = gun?.AmmoCount.ToString();
}
