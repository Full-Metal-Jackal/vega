using TMPro;
using UnityEngine;

namespace Speech
{
	public class PassiveSpeech : MonoBehaviour
	{
		[field: SerializeField]
		public Transform SpeechPosition { get; private set; }

		public TextMeshPro text;

		private float life;

		private void Awake()
		{
			if (!SpeechPosition)
				SpeechPosition = transform;
		}

		private void Start()
		{
			text.enabled = false;
		}

		private void Update()
		{
			if (life > 0)
				text.enabled = (life -= Time.deltaTime) > 0;
		}

		public void Speak(string line) => Speak(line, GetSpeechTime(line));

		/// <summary>
		/// Starts speech animation that floats some time.
		/// </summary>
		/// <param name="line">The line to say.</param>
		/// <param name="time">The time the line will float in the air. Pass value lower than zero to disable automatic disappearance.</param>
		public void Speak(string line, float time)
		{
			life = time;

			text.text = line;
			text.enabled = true;
		}

		private const float secondsPerCharacter = .1f;
		public float GetSpeechTime(string line) => line.Length * secondsPerCharacter;
	}
}
