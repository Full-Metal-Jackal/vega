namespace UI.HUD
{
	public class PlayerHealthBar : HorizontalProgressBar
	{
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
			Value = player.Health / player.MaxHealth;
		}
	}
}
