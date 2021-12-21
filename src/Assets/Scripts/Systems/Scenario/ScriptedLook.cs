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
		public void LookAt(Mob mob) => LookAt(mob.transform.position + Vector3.up*mob.AimHeight);

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

		public void Cease()
		{
			active = false;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;

			float size = .2f;

			Gizmos.DrawLine(
				mob.transform.position + mob.AimHeight * Vector3.up,
				lookAtTarget
			);

			Gizmos.DrawWireSphere(lookAtTarget, size);

			Gizmos.DrawLine(
				lookAtTarget + size * Vector3.back,
				lookAtTarget + size * Vector3.forward
			);
			Gizmos.DrawLine(
				lookAtTarget + size * Vector3.left,
				lookAtTarget + size * Vector3.right
			);
		}
	}
}
