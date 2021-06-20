using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetsMask; //For enemies
	public LayerMask obstacleMask; //For every static object which block view

	public float meshRes;

	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	private void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
		StartCoroutine("FindTargetDelay", .2f);
	}

	void LateUpdate()
	{
		DrawFieldOfView();
	}


	IEnumerator FindTargetDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}
	
	/*
	 * Find targets inside view radius
	 */
	void FindVisibleTargets()
	{

	}

	/*
	 * Convert Unity angle to trigonometry angle
	 */
	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	/*
	 * Draw raycasts which form vision
	 */
	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshRes);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>(); 

		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
			ViewCastInfo newViewCast = ViewCast(angle);
			viewPoints.Add(newViewCast.point);
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];
		vertices[0] = Vector3.zero;

		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
			if (i < vertexCount - 2)
			{
				vertices[i + 1] = viewPoints[i];
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}

	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;


		if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
		{
			Debug.DrawLine(transform.position, hit.point, Color.white);
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			Debug.DrawLine(transform.position, transform.position + dir * viewRadius, Color.green);
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}
	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dist;
		public float angle;

		public ViewCastInfo(bool hit, Vector3 point, float dist, float angle)
		{
			this.hit = hit;
			this.point = point;
			this.dist = dist;
			this.angle = angle;
		}
	}
}
