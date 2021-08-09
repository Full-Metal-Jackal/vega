using UnityEngine;

namespace Scenario
{
	public class ScriptedLook : ScenarioScript
	{
		[SerializeField]
		private Mob mob;

		private bool active = false;

		private Vector3 lookAtTarget;

		[SerializeField]
		private float lookAtTolerance = .5f;

		[SerializeField]
		private float lookSmoothing = .1f;
		private Vector3 lookVelocity;

		public void LookAt(GameObject target) => LookAt(target.transform.position);

		public void LookAt(Vector3 target)
		{
			lookAtTarget = target;
			lookVelocity = Vector3.zero;
			active = true;
		}

		private void FixedUpdate()
		{
			if (!active)
				return;

			mob.AimPos = Vector3.SmoothDamp(
				mob.AimPos,
				lookAtTarget,
				ref lookVelocity,
				lookSmoothing
			);
			mob.TurnTo(Time.fixedDeltaTime, mob.AimDir);

			if (Vector3.Distance(mob.AimPos, lookAtTarget) < lookAtTolerance)
			{
				Finish();
				active = false;
			}
		}
	}
}
