using UnityEngine;

namespace TriggerSystem
{
	public class LevelChangeTrigger : Trigger
	{
		[SerializeField]
		private string sceneName;

		public void ChangeScene(string sceneName) =>
			Game.ChangeLevel(sceneName);
		
		public void ChangeScene() =>
			Game.ChangeLevel(sceneName);
	}
}
