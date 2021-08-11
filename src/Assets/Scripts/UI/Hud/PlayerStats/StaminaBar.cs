using System;
using UnityEngine;

namespace UI
{
	public class StaminaBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;
		
		private Mob player;

		private void Start() =>
			PlayerController.Instance.OnPossesed += (player) =>
			{
				if (this.player)
					player.OnStaminaChanged -= StaminaChangedHandler;
				
				this.player = player;
				player.OnStaminaChanged += StaminaChangedHandler;
			};
		
		private void StaminaChangedHandler()
		{
			float staminaRatio = Math.Min(player.Stamina / player.MaxStamina, 1f);
			rectTransform.localPosition = new Vector3(-rectTransform.rect.width * (1 - staminaRatio), 0f, 0f);
		}
	}
}
