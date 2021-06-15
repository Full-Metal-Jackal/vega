using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	public class UIGridRenderer : UIGraphics
	{
		public Dictionary<Vector2Int, bool> grid = new Dictionary<Vector2Int, bool>();

		public float thickness = 1f;

		public bool background = true;

		private float cellSize = 64f;
		public float CellSize
		{
			get => cellSize;
			set
			{
				cellSize = value;
				SetVerticesDirty();
			}
		}
		
		public Color lineColor = new Color(0f, 1f, 0f);
		public Color highlightedLineColor = new Color(0f, 1f, 0f);

		public Color backgroundColor = new Color(0f, 1f, 0f);
		public Color highlightedBackgroundColor = new Color(0f, 1f, 0f);

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			base.OnPopulateMesh(vh);

			foreach (KeyValuePair<Vector2Int, bool> pair in grid)
				DrawCell(vh, pair.Key, pair.Value);
		}

		protected void DrawCell(VertexHelper vh, Vector2Int cell, bool highlighted = false)
		{
			Color currentColor;
			Vector2 cellOffset = new Vector2(CellSize, CellSize) * (cell);

			if (background)
			{
				currentColor = highlighted ? highlightedBackgroundColor : backgroundColor;
				DrawQuad(
					vh,
					Vector2.zero + cellOffset,
					new Vector2(0, CellSize) + cellOffset,
					new Vector2(CellSize, CellSize) + cellOffset,
					new Vector2(CellSize, 0) + cellOffset,
					currentColor
					);
			}

			currentColor = highlighted ? highlightedLineColor : lineColor;

			float thicknessOffset = thickness / Mathf.Sqrt(2);

			int index = vh.currentVertCount - 1;
			DrawQuad(
				vh,
				cellOffset,
				new Vector2(thicknessOffset, thicknessOffset) + cellOffset,
				new Vector2(CellSize - thicknessOffset, thicknessOffset) + cellOffset,
				new Vector2(CellSize, 0) + cellOffset,
				currentColor
				);

			AppendQuad(
				vh, index + 4, index + 3,
				new Vector2(CellSize - thicknessOffset, CellSize - thicknessOffset) + cellOffset,
				new Vector2(CellSize, CellSize) + cellOffset,
				currentColor
				);

			AppendQuad(
				vh, index + 6, index + 5,
				new Vector2(thicknessOffset, CellSize - thicknessOffset) + cellOffset,
				new Vector2(0, CellSize) + cellOffset,
				currentColor
				);

			JoinQuad(vh, index + 8, index + 7, index + 2, index + 1);
		}

		private void AddCellNoUpdate(Vector2Int cell, bool highlight)
		{
			Debug.Log(cell);

			if (grid.ContainsKey(cell))
				return;
			
			grid.Add(cell, highlight);
		}

		public void AddCell(Vector2Int cell, bool highlight = false)
		{
			AddCellNoUpdate(cell, highlight);
			SetVerticesDirty();
		}

		public void AddCells(IEnumerable<Vector2Int> cells, bool highlight = false)
		{
			foreach (Vector2Int cell in cells)
				AddCellNoUpdate(cell, highlight);
			SetVerticesDirty();
		}
		public void AddCells(Circuitry.Shape shape, bool highlight = false) => AddCells(shape.Cells, highlight);

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
		public void SetHighlight(Circuitry.Shape shape, bool highlight = false) => SetHighlight(shape.Cells, highlight);
	}
}
