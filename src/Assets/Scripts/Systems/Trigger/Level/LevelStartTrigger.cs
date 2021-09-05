using UnityEngine;

namespace TriggerSystem
{
	public class LevelStartTrigger : Trigger
	{
		private bool activated = false;

		// Actually, level start trigger should be triggered at the very first frame of the game but not before it,
		// allowing scripts relying on Start call to load properly before the game starts.
		private void Update()
		{
			if (!activated)
			{
				activated = true;
				Activate();
			}
			enabled = false;
		}
	}
}
