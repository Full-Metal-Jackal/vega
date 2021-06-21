using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public class CircuitCooldownOverlay : MonoBehaviour
	{
		private RectTransform rectTransform;
		private float cooldown = 0;
		private float initialCooldown = 0;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			gameObject.SetActive(false);
		}

		public void Activate(float cooldown)
		{
			initialCooldown = cooldown;
			this.cooldown = initialCooldown;
			gameObject.SetActive(true);
		}

		private void Update()
		{
			cooldown -= Time.deltaTime;
			if (cooldown <= 0)
				gameObject.SetActive(false);

			float fraction = cooldown / initialCooldown;
			rectTransform.localScale = new Vector3(1f, fraction, 1f);
		}
	}
}
