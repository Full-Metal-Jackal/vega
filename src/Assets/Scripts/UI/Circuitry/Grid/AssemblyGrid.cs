using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class AssemblyGrid : CellGridWidget, IDropHandler
	{
		[SerializeField]
		private AssemblyWidget assemblyWidget;

		public Vector2Int Selected { get; protected set; }

		protected readonly Dictionary<Vector2Int, CellWidget> grid = new Dictionary<Vector2Int, CellWidget>();

		private void DeselectPrevious(Shape shape)
		{
			foreach (Vector2Int cell in shape.Cells)
				if (GetCellWidget(Selected + cell) is CellWidget widget)
					widget.SetHighlight(false);
		}

		public void OnGhostHover(GhostCircuitWidget ghost, Vector2Int gridCell)
		{
			if (Selected == gridCell)
				return;

			Shape shape = ghost.Source.Circuit.BoundCircuit.Shape;
			DeselectPrevious(shape);

			Selected = gridCell;

			IEnumerable<Circuitry.Circuit> toIgnore = new HashSet<Circuitry.Circuit> { ghost.Source.Circuit.BoundCircuit };
			bool error = !assemblyWidget.BoundAssembly.grid.DoesFit(shape, gridCell, toIgnore);

			foreach (Vector2Int cell in shape.Cells)
				if (GetCellWidget(Selected + cell) is CellWidget widget)
					widget.SetHighlight(true, error);
		}

		public void OnGhostHover(GhostCircuitWidget ghost, Vector2 position)
		{
			Vector2Int cell = GetCell(position);
			OnGhostHover(ghost, cell);
		}

		public void OnGhostDestroyed(GhostCircuitWidget ghost)
		{
			DeselectPrevious(ghost.Source.Circuit.BoundCircuit.Shape);
		}

		public Vector2Int GetCell(Vector2 position)
		{
			Vector2 scale = Game.circuitConstructor.Viewport.Content.localScale;

			position -= (Vector2)transform.position + (RectTransform.rect.min * scale);
			position /= Layout.cellSize * scale;

			Vector2Int cell = new Vector2Int(
				Mathf.FloorToInt(position.x),
				(shape.Height - 1) - Mathf.FloorToInt(position.y)
				);
			return cell;
		}

		protected override CellWidget CreateCell(Vector2Int cell)
		{
			CellWidget widget = base.CreateCell(cell);
			grid.Add(cell, widget);
			return widget;
		}

		public CellWidget GetCellWidget(Vector2Int cell) => grid.ContainsKey(cell) ? grid[cell] : null;

		public void OnDrop(PointerEventData eventData)
		{
			if (!eventData.pointerDrag.TryGetComponent(out DraggableCircuitWidget draggable))
				return;

			Vector2Int cell = GetCell(eventData.position + draggable.GripOffset);
			draggable.DropOnAssembly(assemblyWidget, cell);
		}
	}
}
