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

		/// <summary>
		/// Makes the mob match its model transforms so it doesn't set off after timelined animations without baked poistions.
		/// </summary>
		/// <param name="mob">The mob whose transform needs to be updated.</param>
		public void ResetMobTransform(MobAnimationHandler animationHandler)
		{
			Mob mob = animationHandler.GetComponentInParent<Mob>();
			if (!mob)
				throw new System.Exception($"MobAnimationHandler {animationHandler} was not assigned as a child of a mob.");

			mob.transform.position = animationHandler.transform.position;
			mob.transform.rotation = animationHandler.transform.rotation;

			animationHandler.transform.localPosition = Vector3.zero;
			animationHandler.transform.localRotation = Quaternion.identity;
		}
	}
}
