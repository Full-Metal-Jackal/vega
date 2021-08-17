using UnityEngine;

namespace Scenario
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(LineRenderer))]
	public class ScriptedWalkPreview : MonoBehaviour
	{
		[SerializeField]
		private LineRenderer line;

		[SerializeField]
		private ScriptedWalk scriptedWalk;

		private void Update()
		{
			scriptedWalk.CreatePath();
			Vector3[] path = scriptedWalk.Path;

			line.positionCount = path.Length;
			line.SetPositions(path);
		}

		private void Start()
		{
			if (Application.isPlaying)
				Destroy(gameObject);
		}
	}
}
