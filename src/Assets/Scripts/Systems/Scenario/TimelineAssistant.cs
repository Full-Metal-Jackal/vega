using UnityEngine;
using UnityEngine.Playables;

namespace Scenario
{
	[RequireComponent(typeof(PlayableDirector))]
	public class TimelineAssistant : MonoBehaviour
	{
		private PlayableDirector director;

		private void Awake() =>
			director = GetComponent<PlayableDirector>();

		public double Speed
		{
			get => director.playableGraph.GetRootPlayable(0).GetSpeed();
			set => director.playableGraph.GetRootPlayable(0).SetSpeed(value);
		}

		public void Pause() =>
			Speed = 0;

		public void Resume() =>
			Speed = 1;
	}
}
