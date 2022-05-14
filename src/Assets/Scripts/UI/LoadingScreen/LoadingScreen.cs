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

			Debug.Log("Opening the loading screen...");
			this.loading = loading;
		}

		public void FinishLoading()
		{
			finished = true;
			progressBar.Text.text = "Continue";
		}

		private void OnGUI()
		{
			if (finished)
				return;

			progressBar.Progress = loading.progress / LevelLoader.neededProgress;

			const int maxDots = 3;
			const float drummingSpeed = 3f;
			progressBar.Text.text = "Loading" + new string('.', 1 + (Mathf.FloorToInt(Time.time * drummingSpeed) % maxDots));

			if (loading.progress >= LevelLoader.neededProgress)
				FinishLoading();
		}

		public void ContinuePressed()
		{
			if (!finished)
				return;
				
			loading.allowSceneActivation = true;
			Close();
		}

		public void Close()
		{
			gameObject.SetActive(false);  // Might destroy the entire object as well.
			Game.Paused = false;

			Debug.Log("Closing the loading screen...");
		}
	}
}
