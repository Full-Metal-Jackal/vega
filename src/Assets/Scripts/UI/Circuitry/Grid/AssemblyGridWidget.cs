using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class AssemblyGridWidget : CellGridWidget
	{
		[SerializeField]
		private AssemblyWidget assemblyWidget;
		public AssemblyWidget AssemblyWidget => assemblyWidget;

		public Vector2Int Selected { get; protected set; }

		protected readonly Dictionary<Vector2Int, CellWidget> grid = new Dictionary<Vector2Int, CellWidget>();

		public void OnGhostHover(CircuitGhostWidget ghost, Vector2Int gridCell)
		{
			Circuit circuit = ghost.CircuitWidgetPrefab.GetComponent<Circuit>();

			Debug.Log($"CUM PISS {gridCell}");

			foreach (Vector2Int cell in circuit.shape.Cells)
				GetCellWidget(Selected + cell).SetHighlight(false);

			Selected = gridCell;
			foreach (Vector2Int cell in circuit.shape.Cells)
				GetCellWidget(Selected + cell).SetHighlight(true);

		}

		protected override GameObject CreateCell(Vector2Int cell)
		{
			GameObject cellObject = base.CreateCell(cell);

			AssemblyCellWidget widget = cellObject.GetComponent<AssemblyCellWidget>();
			widget.Setup(cell, this);
			grid.Add(cell, widget);

			return cellObject;
		}

		public CellWidget GetCellWidget(Vector2Int cell) => grid.ContainsKey(cell) ? grid[cell] : null;

		public void OnGhostDropped(CircuitGhostWidget ghost, Vector2Int cell)
		{

		}
	}
}
