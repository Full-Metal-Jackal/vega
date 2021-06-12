using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UILineRenderer : UIGraphics
	{
		public List<Vector2> dots = new List<Vector2>();

		public float thickness = 4f;

		public Color lineColor = new Color(0f, 1f, 0f);
		public Color highlightedLineColor = new Color(0f, 1f, 0f);

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			base.OnPopulateMesh(vh);
			DrawLine(vh);
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

		protected void AppendAngled(VertexHelper vh, Vector2 from, Vector2 to, Vector2 post)
		{
			float toAngle = Vector3.SignedAngle(post - to, from - to, Vector3.forward);
			Vector2 toOffset = GetUniformOffset(post - to, toAngle / 2);

			int vxCount = vh.currentVertCount;
			AppendQuad(
				vh,
				vxCount - 1,
				vxCount - 2,
				to + toOffset,
				to - toOffset,
				lineColor
				);
		}

		protected void DrawLine(VertexHelper vh)
		{
			if (dots.Count < 2)
				return;

			if (dots.Count == 2)
			{
				DrawLine(vh, dots[0], dots[1], thickness, lineColor);
				return;
			}

			// <TODO> Still not optimized for 3 vertices case for it generates additional two points at the start segment.

			DrawAngled(vh, dots[0], dots[1], dots[2]);
			for (int i = 1; i <= dots.Count - 4; i++)
				AppendAngled(vh, dots[i], dots[i + 1], dots[i + 2]);

			DrawAngled(vh, dots[dots.Count - 1], dots[dots.Count - 2], dots[dots.Count - 3]);
			int index = vh.currentVertCount - 6;
			JoinQuad(vh, index + 1, index, index + 5, index + 4);

			Debug.Log(vh.currentVertCount);
		}
	}
}
