using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UIGridRenderer : Graphic
	{
		public global::Circuitry.Grid grid;

		public float thickness = 4f;

		public float cellSize = 32f;
		
		public Color lineColor = new Color(0f, 1f, 0f);
		public Color highlightedLineColor = new Color(0f, 1f, 0f);

		public Color backgroundColor = new Color(0f, 1f, 0f);
		public Color highlightedBackgroundColor = new Color(0f, 1f, 0f);

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();

			if (!grid)
				return;

			foreach (Vector2Int cell in grid.cells.Keys)
				DrawCell(vh, cell);
		}

		protected void DrawCell(VertexHelper vh, Vector2Int cell)
		{
			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = lineColor;

			float thicknessOffset = thickness / Mathf.Sqrt(2);
			Vector3 cellOffset = new Vector3(cellSize * cell.x, cellSize * cell.y);

			vertex.position = new Vector3(0, 0) + cellOffset;
			vh.AddVert(vertex);
			vertex.position = new Vector3(cellSize, 0) + cellOffset;
			vh.AddVert(vertex);
			vertex.position = new Vector3(cellSize, cellSize) + cellOffset;
			vh.AddVert(vertex);
			vertex.position = new Vector3(0, cellSize) + cellOffset;
			vh.AddVert(vertex);

			vertex.position = new Vector3(thicknessOffset, thicknessOffset) + cellOffset;
			vh.AddVert(vertex);
			vertex.position = new Vector3(cellSize - thicknessOffset, thicknessOffset) + cellOffset;
			vh.AddVert(vertex);
			vertex.position = new Vector3(cellSize - thicknessOffset, cellSize - thicknessOffset) + cellOffset;
			vh.AddVert(vertex);
			vertex.position = new Vector3(thicknessOffset, cellSize - thicknessOffset) + cellOffset;
			vh.AddVert(vertex);

			vh.AddTriangle(0, 1, 4);
			vh.AddTriangle(1, 4, 5);

			vh.AddTriangle(1, 2, 5);
			vh.AddTriangle(2, 5, 6);

			vh.AddTriangle(2, 3, 6);
			vh.AddTriangle(3, 6, 7);

			vh.AddTriangle(3, 0, 7);
			vh.AddTriangle(0, 7, 4);
		}
	}
}
