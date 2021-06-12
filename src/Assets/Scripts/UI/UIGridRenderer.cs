using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public class UIGridRenderer : Graphic
	{
		public Dictionary<Vector2Int, bool> grid = new Dictionary<Vector2Int, bool>();

		public float thickness = 1f;

		public float cellSize = 32f;
		
		public Color lineColor = new Color(0f, 1f, 0f);
		public Color highlightedLineColor = new Color(0f, 1f, 0f);

		public Color backgroundColor = new Color(0f, 1f, 0f);
		public Color highlightedBackgroundColor = new Color(0f, 1f, 0f);

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();

			foreach (KeyValuePair<Vector2Int, bool> pair in grid)
				DrawCell(vh, pair.Key, pair.Value ? highlightedLineColor : lineColor);
		}

		protected void DrawCell(VertexHelper vh, Vector2Int cell, Color color)
		{
			UIVertex vertex = UIVertex.simpleVert;
			vertex.color = color;

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

		private void AddCellNoUpdate(Vector2Int cell, bool highlight)
		{
			if (grid.ContainsKey(cell))
				return;
			
			Debug.Log($"{cell} cell has been added");

			grid.Add(cell, highlight);
		}

		public void AddCell(Vector2Int cell, bool highlight = false)
		{
			AddCellNoUpdate(cell, highlight);
			SetVerticesDirty();
		}

		public void AddCells(IEnumerable<Vector2Int> cells, bool highlight = false)
		{
			Debug.Log($"{new HashSet<Vector2Int>(cells).Count} cells have been added");

			foreach (Vector2Int cell in cells)
				AddCellNoUpdate(cell, highlight);
			SetVerticesDirty();
		}
		public void AddCells(Circuitry.Shape shape, bool highlight = false) => AddCells(shape.cells, highlight);

		private void SetHighlightNoUpdate(Vector2Int cell, bool highlight)
		{
			if (!grid.ContainsKey(cell))
				return;
			grid[cell] = highlight;
		}

		public void SetHighlight(Vector2Int cell, bool highlight)
		{
			SetHighlightNoUpdate(cell, highlight);
			// SetMaterialDirty(); <TODO> check if this is enough to update highlight
			SetAllDirty();
		}

		public void SetHighlight(IEnumerable<Vector2Int> cells, bool highlight)
		{
			foreach (Vector2Int cell in cells)
				SetHighlightNoUpdate(cell, highlight);
			SetAllDirty();
		}
		public void SetHighlight(Circuitry.Shape shape, bool highlight = false) => SetHighlight(shape.cells, highlight);
	}
}
