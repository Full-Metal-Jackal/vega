using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UILineRenderer : Graphic
	{
		public List<Vector2> dots = new List<Vector2>();
		private int vertices = 0;

		public float thickness = 4f;

		public Color lineColor = new Color(0f, 1f, 0f);
		public Color highlightedLineColor = new Color(0f, 1f, 0f);

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			vertices = 0;
			DrawLine(vh);
		}

		protected void DrawSingleLine(VertexHelper vh, Vector2 from, Vector2 to)
		{
			Vector2 thicknessOffset = Quaternion.Euler(0, 0, 90) * ((to - from).normalized * thickness);
			DrawQuad(
				vh,
				from + thicknessOffset,
				to + thicknessOffset,
				to - thicknessOffset,
				from - thicknessOffset
				);
		}

		private Vector2 GetUniformOffset(Vector2 direction, float angle) => Quaternion.Euler(0, 0, angle) * direction.normalized * thickness / Mathf.Sin(angle * Mathf.Deg2Rad);

		protected void DrawAngled(VertexHelper vh, Vector2 pre, Vector2 from, Vector2 to, Vector2 post)
		{
			float fromAngle = -Vector3.SignedAngle(to - from, pre - from, Vector3.forward);
			Vector2 fromOffset = GetUniformOffset(from - pre, fromAngle / 2);

			float toAngle = Vector3.SignedAngle(post - to, from - to, Vector3.forward);
			Vector2 toOffset = GetUniformOffset(post - to, toAngle / 2);

			DrawQuad(
				vh,
				from - fromOffset,
				from + fromOffset,
				to + toOffset,
				to - toOffset
				);
		}

		protected void DrawAngled(VertexHelper vh, Vector2 from, Vector2 to, Vector2 post)
		{
			const float straightEdge = 90f;
			Vector2 fromOffset = Quaternion.Euler(0, 0, straightEdge) * ((to - from).normalized * thickness);

			float toAngle = Vector3.SignedAngle(post - to, from - to, Vector3.forward);
			Vector2 toOffset = GetUniformOffset(post - to, toAngle / 2);

			DrawQuad(
				vh,
				from - fromOffset,
				from + fromOffset,
				to + toOffset,
				to - toOffset
				);
		}

		protected void DrawLine(VertexHelper vh)
		{
			if (dots.Count < 2)
				return;

			if (dots.Count == 2)
			{
				DrawSingleLine(vh, dots[0], dots[1]);
				return;
			}

			// Am clueless about how to make it less kludgy. – Ocelot
			DrawAngled(vh, dots[0], dots[1], dots[2]);
			DrawAngled(vh, dots[dots.Count - 1], dots[dots.Count - 2], dots[dots.Count - 3]);
			for (int i = 0; i <= dots.Count - 4; i++)
				DrawAngled(vh, dots[i], dots[i + 1], dots[i + 2], dots[i + 3]);
		}

		protected void DrawQuad(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color color)
		{
			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = color;

			vertex.position = v1;
			vh.AddVert(vertex);
			vertex.position = v2;
			vh.AddVert(vertex);
			vertex.position = v3;
			vh.AddVert(vertex);
			vertex.position = v4;
			vh.AddVert(vertex);

			vh.AddTriangle(vertices, vertices + 1, vertices + 2);
			vh.AddTriangle(vertices + 2, vertices + 3, vertices);

			vertices += 4;
		}

		protected void DrawQuad(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) => DrawQuad(vh, v1, v2, v3, v4, lineColor);

		protected void DrawTriangle(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
		{
			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = color;

			vertex.position = v1;
			vh.AddVert(vertex);
			vertex.position = v2;
			vh.AddVert(vertex);
			vertex.position = v3;
			vh.AddVert(vertex);

			vh.AddTriangle(vertices, vertices + 1, vertices + 2);

			vertices += 3;
		}
		protected void DrawTriangle(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3) => DrawTriangle(vh, v1, v2, v3, lineColor);
	}
}
