using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriggerSystem
{
	public class Timer : Trigger
	{
		/// <summary>
		/// How much time should pass befoare the trigger activates.
		/// </summary>
		[SerializeField]
		private float Delay;

		private bool countdownActivated = false;
		private float currentTime;

		public void StartTimer() => StartTimer(Delay);

		public void StartTimer(float time)
		{
			countdownActivated = true;
			currentTime = time;
		}

		private void Update()
		{
			if (!countdownActivated)
				return;

			if ((currentTime -= Time.deltaTime) > 0f)
				return;

			countdownActivated = false;
			Activate();
		}
	}
}
