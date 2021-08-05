using UnityEngine;

namespace Input
{
	public class PlayerInput : MonoSingleton<PlayerInput>
	{
		public static InputActions Actions { get; private set; }

		public static bool WorldInputEnabled
		{
			get => Actions.World.enabled;
			set
			{
				if (value)
					Actions.World.Enable();
				else
					Actions.World.Disable();
			}
		}

		public static bool UiInputEnabled
		{
			get => Actions.UI.enabled;
			set
			{
				if (value)
					Actions.UI.Enable();
				else
					Actions.UI.Disable();
			}
		}

		private void Awake()
		{
			Actions = new InputActions();
		}

		public void UpdateInput()
		{
			WorldInputEnabled = !(Game.Paused || Game.PlayingScene);
			UiInputEnabled = Game.Paused;
		}
	}
}
