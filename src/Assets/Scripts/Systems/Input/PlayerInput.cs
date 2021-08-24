using UnityEngine;

namespace Input
{
	public static class PlayerInput
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

		public static void Initialize()
		{
			Actions = new InputActions();
		}

		public static void UpdateInput()
		{
			WorldInputEnabled = !(Game.Paused || Game.PlayingScene);
			UiInputEnabled = Game.Paused;
		}
	}
}
