using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scenario
{
	public class ScriptedWalk : ScenarioScript
	{
		[SerializeField]
		private Mob mob;

		private bool activated = false;

		[SerializeField]
		private List<Transform> pathNodes;

		[field: SerializeField]
		private Queue<Vector3> path = new Queue<Vector3>();
		[SerializeField]
		private float pathSmoothing = 5f;
		public Vector3[] Path => path.ToArray();

		[SerializeField]
		private float approachDistance = .1f;

		[SerializeField]
		private bool forceToLookAtMovementDirection = true;

		private Vector3 currentTarget;

		[SerializeField]
		private MovementType movementType = MovementType.Walking;

		public void CreatePath()
		{
			Vector3[] nodePositions = pathNodes.Select(node => node.position).ToArray();

			path.Clear();
			foreach (Vector3 pos in Utils.SmoothLine(nodePositions, pathSmoothing))
				path.Enqueue(pos);
		}

		public void Perform()
		{
			if (pathNodes.Count < 0)
				throw new System.Exception($"{this} has been activated without a path assigned.");

			CreatePath();

			currentTarget = path.Dequeue();
			mob.MovementType = movementType;
			activated = true;
		}

		private void FixedUpdate()
		{
			if (!activated)
				return;

			Vector3 mobPos = mob.transform.position;

			Vector3 horDir = currentTarget - mobPos;
			horDir.y = 0;
			mob.Move(Time.fixedDeltaTime, horDir.normalized);

			if (Utils.HorizontalDistance(mobPos, currentTarget) > approachDistance)
				return;

			if (!path.Any())
			{
				Finish();
				return;
			}

			currentTarget = path.Dequeue();
			if (forceToLookAtMovementDirection)
				mob.AimPos = currentTarget;
		}

		protected override void Finish()
		{
			mob.Move(Time.deltaTime, Vector3.zero);
			activated = false;

			base.Finish();
		}

		private void OnDrawGizmos()
		{
			CreatePath();
			Vector3[] gizmoPath = Path;

			Gizmos.color = Color.green;
			for (int i= 0; i < gizmoPath.Length - 1; i++)
				Gizmos.DrawLine(gizmoPath[i], gizmoPath[i+1]);

			for (int i= 0; i < gizmoPath.Length; i++)
			{
				Gizmos.color = Color.yellow;
				float size = .05f;

				if (i == 0 || i == gizmoPath.Length - 1)
				{
					Gizmos.color = Color.green;
					size = .1f;
				}

				Gizmos.DrawWireSphere(gizmoPath[i], size);
			}

			Gizmos.color = Color.green;
			foreach (Transform transform in pathNodes)
				Gizmos.DrawWireSphere(transform.position, .05f);
		}
	}
}
