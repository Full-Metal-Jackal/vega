using UnityEngine;

namespace TriggerSystem
{
	public class LevelChanger : Trigger
	{
		[SerializeField]
		private string sceneName;

		public void ChangeScene(string sceneName) =>
			LevelLoader.Instance.ChangeLevel(sceneName);
		
		public void ChangeScene() =>
			LevelLoader.Instance.ChangeLevel(sceneName);
	}
}
