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
		private float currentCooldown = 0;
		private float initialCooldown = float.PositiveInfinity;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			gameObject.SetActive(false);
		}

		public void StartCooldownAnimation(float cooldown)
		{
			initialCooldown = cooldown;
			currentCooldown = initialCooldown;
			gameObject.SetActive(true);
			Debug.Log($"{gameObject}: CD for {cooldown} secs began.");
		}

		private void Update()
		{
			Debug.Log($"CD for {currentCooldown} remains...");
			currentCooldown -= Time.deltaTime;
			if (currentCooldown <= 0)
				gameObject.SetActive(false);

			float fraction = currentCooldown / initialCooldown;
			rectTransform.localScale = new Vector3(1f, fraction, 1f);
		}
	}
}
