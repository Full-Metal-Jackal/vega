using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;


namespace AI
{
	public class NavMeshPathVisualizer : MonoBehaviour
	{
		LineRenderer line;
		NavMeshAgent agent;

		void Start()
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
