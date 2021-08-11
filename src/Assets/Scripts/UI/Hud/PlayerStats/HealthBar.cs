using System;
using UnityEngine;

namespace UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rectTransform;

		private Mob player;

		private void Start() =>
			PlayerController.Instance.OnPossesed += (player) =>
			{
				if (this.player)
					this.player.OnHealthChanged -= HealthChangedHandler;
				
				this.player = player;
				this.player.OnHealthChanged += HealthChangedHandler;
			};

		private void HealthChangedHandler()
		{
			float healthRatio = Math.Min(player.Health / player.MaxHealth, 1f);
			rectTransform.localPosition = new Vector3(-rectTransform.rect.width * (1 - healthRatio), 0f, 0f);
		}
	}
}
