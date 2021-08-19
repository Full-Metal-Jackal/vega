using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI
{
	public class StaminaBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private List<AlphaIndicator> indicators;

		private Mob player;

		private void Start()
		{
			enabled = false;
			PlayerController.Instance.OnPossessed += (player) =>
			{
				this.player = player;
				enabled = true;
			};
		}

		// NOTE: StaminaBar uses Update() logic instead of an event
		// because the value of stamina will be updated almost every frame e.g. when user holds down the shift button,
		// and calling an event delegate almost every frame is much more expensive than this
		private void Update()
		{
			float toFlash = Mathf.Clamp01(player.Stamina / player.MaxStamina) * indicators.Count;

			Debug.Log(Mathf.Clamp01(player.Stamina / player.MaxStamina));

			foreach (AlphaIndicator indicator in indicators)
			{
				indicator.Value = toFlash;
				toFlash -= 1f;
			}
		}
	}
}
