using System;
using UnityEngine;

namespace UI
{
	public class StaminaBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;
		
		private Mob player;

		private void Start()
		{
			enabled = false;
			PlayerController.Instance.OnPossesed += (player) =>
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
			float staminaRatio = Math.Min(player.Stamina / player.MaxStamina, 1f);
			rectTransform.localPosition = new Vector3(-rectTransform.rect.width * (1 - staminaRatio), 0f, 0f);
		}
	}
}
