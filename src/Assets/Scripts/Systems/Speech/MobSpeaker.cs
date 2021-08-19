using TMPro;
using UnityEngine;

namespace Speech
{
	public class MobSpeaker : MonoBehaviour
	{
		[field: SerializeField]
		public Transform SpeechPosition { get; private set; }

		public TextMeshPro text;

		private float life;

		private Transform disappearAwayFrom;
		private float disappearAtDistance;

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

			if (disappearAwayFrom && Vector3.Distance(transform.position, disappearAwayFrom.position) > disappearAtDistance)
			{
				life = 0;
				disappearAwayFrom = null;
				text.enabled = false;
			}
		}

		public void Speak(string line) => Speak(line, GetSpeechTime(line));

		/// <summary>
		/// Starts speech animation that floats some time.
		/// </summary>
		/// <param name="line">The line to say.</param>
		/// <param name="time">The time the line will float in the air. Pass value lower than zero to disable automatic disappearance.</param>
		public void Speak(string line, float time, bool additive = false)
		{
			if (additive)
				time += GetSpeechTime(line);
			life = time;

			text.text = line;
			text.enabled = true;
		}

		public void DisappearAtDistance(Transform subjectTransform, float distance = 2f)
		{
			disappearAwayFrom = subjectTransform;
			disappearAtDistance = distance;
		}

		private const float secondsPerCharacter = .1f;
		public float GetSpeechTime(string line) => line.Length * secondsPerCharacter;
	}
}
