namespace UI.HUD
{
	public class PlayerHealthBar : HorizontalProgressBar
	{
		private Mob player;

		private void Awake() =>
			PlayerController.Instance.OnPossessed += SetPlayer;

		private void SetPlayer(Mob mobPlayer)
		{
			if (player)
				player.OnHealthChanged -= HealthChangedHandler;

			player = mobPlayer;
			player.OnHealthChanged += HealthChangedHandler;
		}

		private void HealthChangedHandler()
		{
			Value = player.Health / player.MaxHealth;
		}
	}
}
