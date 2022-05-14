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

		private Vector3 disappearAwayFrom;
		private float disappearAtDistance = 0;

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

			if (disappearAtDistance >= 0 &&
				Utils.HorizontalDistance(transform.position, disappearAwayFrom) > disappearAtDistance
			)
			{
				life = 0;
				disappearAtDistance = 0;
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


		public void DisappearAtDistance(Vector3 interactionSpot) =>
			DisappearAtDistance(interactionSpot, PlayerController.Instance.selectionDistance);

		public void DisappearAtDistance(Vector3 interactionSpot, float distance)
		{
			disappearAwayFrom = interactionSpot;
			disappearAtDistance = distance;
		}

		private const float secondsPerCharacter = .1f;
		public float GetSpeechTime(string line) => line.Length * secondsPerCharacter;
	}
}
