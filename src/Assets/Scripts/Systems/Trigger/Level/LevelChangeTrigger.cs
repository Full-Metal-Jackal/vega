using UnityEngine;

namespace TriggerSystem
{
	public class LevelChangeTrigger : Trigger
	{
		[SerializeField]
		private string sceneName;

		public void ChangeScene(string sceneName) =>
			LevelLoader.Instance.ChangeLevel(sceneName);
		
		public void ChangeScene() =>
			LevelLoader.Instance.ChangeLevel(sceneName);
	}
}
