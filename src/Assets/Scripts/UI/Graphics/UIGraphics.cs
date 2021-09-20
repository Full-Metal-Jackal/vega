using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(CanvasRenderer))]
	public class UIGraphics : MaskableGraphic
	{
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
		}

		public void DrawQuad(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color color)
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

			int index = vh.currentVertCount - 4;
			JoinQuad(vh, index, index + 1, index + 2, index + 3);
		}

		public void AppendQuad(VertexHelper vh, int v1, int v2, Vector2 v3, Vector2 v4, Color color)
		{
			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = color;

			vertex.position = v3;
			vh.AddVert(vertex);
			vertex.position = v4;
			vh.AddVert(vertex);

			int index = vh.currentVertCount - 2;
			vh.AddTriangle(v1, v2, index);
			vh.AddTriangle(index, index + 1, v1);
		}

		public void JoinQuad(VertexHelper vh, int v1, int v2, int v3, int v4)
		{
			vh.AddTriangle(v1, v2, v3);
			vh.AddTriangle(v3, v4, v1);
		}

		public void DrawQuad(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4) => DrawQuad(vh, v1, v2, v3, v4, color);

		public void DrawTriangle(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3, Color color)
		{
			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = color;

			vertex.position = v1;
			vh.AddVert(vertex);
			vertex.position = v2;
			vh.AddVert(vertex);
			vertex.position = v3;
			vh.AddVert(vertex);

			int index = vh.currentVertCount - 3;
			vh.AddTriangle(index, index + 1, index + 2);
		}

		public void DrawTriangle(VertexHelper vh, Vector2 v1, Vector2 v2, Vector2 v3) => DrawTriangle(vh, v1, v2, v3, color);

		protected void DrawLine(VertexHelper vh, Vector2 from, Vector2 to, float thickness, Color color)
		{
			Vector2 thicknessOffset = Quaternion.Euler(0, 0, 90) * ((to - from).normalized * thickness);
			DrawQuad(
				vh,
				from + thicknessOffset,
				to + thicknessOffset,
				to - thicknessOffset,
				from - thicknessOffset,
				color
				);
		}
	}
}
