using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;


namespace AI
{
	public class NavMeshPathVisualizer : MonoBehaviour
	{
		LineRenderer line; //to hold the line Renderer
		Transform target; //to hold the transform of the target
		NavMeshAgent agent; //to hold the agent of this gameObject

		void Start()
		{
			line = GetComponent<LineRenderer>(); //get the line renderer
			agent = GetComponent<NavMeshAgent>(); //get the agent
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
