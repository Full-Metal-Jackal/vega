using DialogueEditor;
using UnityEngine;

namespace UI.Dialogue
{
	public class DialogueWindow : MonoSingleton<DialogueWindow>
	{
		private void Start()
		{
			gameObject.SetActive(false);

			ConversationManager.OnConversationStarted += () => Open();
			ConversationManager.OnConversationEnded += () => Close();
		}

		public void Open()
		{
			Debug.Log("Opening the dialogue window...");
			gameObject.SetActive(true);
			Game.State = GameState.Paused;
			Hud.Instance.Toggle(false);
		}

		public void Close()
		{
			Debug.Log("Closing the dialogue window...");
			gameObject.SetActive(false);
			Game.State = GameState.Normal;
			Hud.Instance.Toggle(true);
		}
	}
}
