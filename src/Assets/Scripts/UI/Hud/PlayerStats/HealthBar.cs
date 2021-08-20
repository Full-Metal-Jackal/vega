using System;
using UnityEngine;

namespace UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform barTransform;

		private Mob player;

		private void Start() =>
			PlayerController.Instance.OnPossessed += (player) =>
			{
				if (this.player)
					this.player.OnHealthChanged -= HealthChangedHandler;
				
				this.player = player;
				this.player.OnHealthChanged += HealthChangedHandler;
			};

		private void HealthChangedHandler()
		{
			float healthRatio = Math.Min(player.Health / player.MaxHealth, 1f);
			barTransform.localPosition = new Vector3(-barTransform.rect.width * (1 - healthRatio), 0f, 0f);
		}
	}
}
