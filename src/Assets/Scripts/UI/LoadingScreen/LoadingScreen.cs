using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Loading
{
	public class LoadingScreen : MonoSingleton<LoadingScreen>
	{
		[SerializeField]
		private LoadingProgressBar progressBar;

		[SerializeField]
		private Button continueButton;

		[SerializeField]
		private Text story;

		[SerializeField]
		private Image levelPreview;

		[SerializeField]
		private Text tip;

		private bool finished;

		AsyncOperation loading;

		public void Start()
		{
			gameObject.SetActive(false);
		}

		public void Open(AsyncOperation loading)
		{
			gameObject.SetActive(true);
			Game.Paused = true;
			ToggleButton(false);

			Debug.Log("Opening the loading screen...");
			this.loading = loading;
		}

		public void FinishLoading()
		{
			finished = true;
			ToggleButton(true);
		}

		private void OnGUI()
		{
			if (finished)
				return;
			
			const float neededProgress = .9f;  // loading.isDone never happens for some reason.

			progressBar.Progress = loading.progress / neededProgress;

			if (loading.progress >= neededProgress)
				FinishLoading();
		}

		public void ToggleButton(bool enabled)
		{
			continueButton.gameObject.SetActive(enabled);
		}

		public void Close()
		{
			gameObject.SetActive(false);
			Game.Paused = false;

			Debug.Log("Closing the loading screen...");
			loading.allowSceneActivation = true;
		}
	}
}
