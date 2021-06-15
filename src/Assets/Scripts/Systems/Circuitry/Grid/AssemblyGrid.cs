using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Circuitry
{
	public class AssemblyGrid
	{
		protected Dictionary<Vector2Int, Circuit> cells = new Dictionary<Vector2Int, Circuit>();
		public IEnumerable<Vector2Int> Cells => cells.Keys.AsEnumerable();

		/// <summary>
		/// Creates empty cells of grid.
		/// </summary>
		/// <param name="cells">Cells of grid that should be created.</param>
		public void CreateEmpty(IEnumerable<Vector2Int> cells)
		{
			foreach (Vector2Int cell in cells)
				this.cells.Add(cell, null);
		}

		public void CreateEmpty(Shape shape) => CreateEmpty(shape.Cells);

		/// <summary>
		/// Attempts to find first cell in the grid that can fit the provided shape.
		/// NOTE: this method is unoptimized and may be rewritten as a 2d implementation of Boyer-Moore algorithm.
		/// </summary>
		/// <param name="shape">The shape to check.</param>
		/// <param name="freeCell">The free cell found.</param>
		/// <returns>false if this grid cannot fit the given shape, true otherwise.</returns>
		public bool FindFirstFree(Shape shape, out Vector2Int freeCell)
		{
			freeCell = Vector2Int.zero;

			if (shape.CellCount <= 0 || shape.CellCount > cells.Count)
				return false;

			foreach (Vector2Int gridCell in cells.Keys)  // .Where((Vector2Int c) => cells[c] is null))
			{
				if (DoesFit(shape, gridCell))
				{
					freeCell = gridCell;
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the shape can fit in the grid cell.
		/// </summary>
		/// <param name="shape">The shape to check.</param>
		/// <param name="gridCell">The cell in the grid to check the shape placement in.</param>
		/// <param name="ignoredCircuits">Circuits that should not be regarded as ones occupying cells.</param>
		/// <returns>true if the shape can fit in the grid cell, false otherwise</returns>
		public bool DoesFit(Shape shape, Vector2Int gridCell, IEnumerable<Circuit> ignoredCircuits)
		{
			foreach (Vector2Int shapeCell in shape.Cells)
				if (!(cells.TryGetValue(gridCell + shapeCell, out Circuit circuit) && (circuit is null || ignoredCircuits.Contains(circuit))))
					return false;
			return true;
		}

		/// <summary>
		/// Checks if the shape can fit in the grid cell.
		/// </summary>
		/// <param name="shape">The shape to check.</param>
		/// <param name="gridCell">The cell in the grid to check the shape placement in.</param>
		/// <returns>true if the shape can fit in the grid cell, false otherwise</returns>
		public bool DoesFit(Shape shape, Vector2Int gridCell) => DoesFit(shape, gridCell, new HashSet<Circuit>());

		/// <summary>
		/// Attempts to place the circuit onto the grid.
		/// </summary>
		/// <param name="circuit">The circuit to place.</param>
		/// <param name="cell">The grid cell to place the circuit in.</param>
		/// <returns>true if the circuit was placed succesfully, false otherwise</returns>
		public bool AddCircuit(Circuit circuit, Vector2Int cell)
		{
			if (cells.Values.Contains(circuit))
				return false;

			if (!DoesFit(circuit.shape, cell))
				return false;

			ForceAddCircuit(circuit, cell);

			return true;
		}

		private void ForceAddCircuit(Circuit circuit, Vector2Int gridCell)
		{
			foreach (Vector2Int shapeCell in circuit.shape.Cells)
				cells[gridCell + shapeCell] = circuit;
		}

		/// <summary>
		/// Attempts to place the circuit in the first available cell of the grid.
		/// </summary>
		/// <param name="circuit">The circuit to place.</param>
		/// <returns>true if the circuit was placed succesfully, false otherwise</returns>
		public bool AddCircuit(Circuit circuit) => FindFirstFree(circuit.shape, out Vector2Int free) && AddCircuit(circuit, free);

		public bool MoveCircuit(Circuit circuit, Vector2Int cell)
		{
			if (!DoesFit(circuit.shape, cell, new HashSet<Circuit> { circuit }))
				return false;

			if (!RemoveCircuit(circuit))
				return false;

			ForceAddCircuit(circuit, cell);

			return true;
		}

		public bool RemoveCircuit(Circuit circuit)
		{
			bool found = false;

			foreach (Vector2Int cell in cells.Keys.Where((Vector2Int c) => cells[c] == circuit))
			{
				cells[cell] = null;
				found = true;
			}

			return found;
		}

		public bool GetCircuit(Vector2Int cell, out Circuit circuit) => cells.TryGetValue(cell, out circuit);

		public static implicit operator Shape(AssemblyGrid grid) => new Shape(grid.cells.Keys);
	}
}
