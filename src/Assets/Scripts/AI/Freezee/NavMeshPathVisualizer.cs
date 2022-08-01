using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;


namespace FreezeeAI
{
	public class NavMeshPathVisualizer : MonoBehaviour
	{
		private LineRenderer line;
		private NavMeshAgent agent;

		private void Awake()
		{
			line = GetComponent<LineRenderer>();
			agent = GetComponent<NavMeshAgent>();
		}

		public void DrawPath(NavMeshPath path)
		{
			line.SetPosition(0, transform.position);
			if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
				return;

			line.positionCount = path.corners.Length;  //set the array of positions to the amount of corners
			line.SetPositions(agent.path.corners);
		}
	}
}
