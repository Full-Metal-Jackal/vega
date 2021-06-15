using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(GridLayoutGroup))]
	public class ConstructorGrid : MonoBehaviour
	{
		public bool Initialized { get; private set; } = false;

		[SerializeField]
		private GameObject cellPrefab;

		private GridLayoutGroup layout;

		protected readonly Dictionary<Vector2Int, CellWidget> grid = new Dictionary<Vector2Int, CellWidget>();

		private void Awake()
		{
			Initialize();
		}

		private bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			layout = GetComponent<GridLayoutGroup>();

			return Initialized = true;
		}

		protected void CreateCell(Vector2Int cell)
		{
			GameObject cellObject = Instantiate(cellPrefab);
			cellObject.transform.SetParent(layout.transform, false);
			grid.Add(cell, cellObject.GetComponent<CellWidget>());
		}

		/// <summary>
		/// Removes every cell from the grid.
		/// </summary>
		public void ClearGrid()
		{

		}

		/// <summary>
		/// Creates grid for a provided shape.
		/// </summary>
		/// <param name="shape"></param>
		public void BuildGrid(Circuitry.Shape shape)
		{
			ClearGrid();

			(shape = shape.GetCopy()).Normalize();

			// Since unity is trash and we cannot directly place our stuff in proper cells,
			// we have to deal with the rectangular shape of the grid,
			// clogging the non-existing cells by calling the SkipCell method.
			layout.constraintCount = shape.Width;
			for (int x = 0; x < shape.Width; x++)
			{
				for (int y = 0; y < shape.Height; y++)
				{
					Vector2Int cell = new Vector2Int(x, y);
					if (shape.HasCell(cell))
						CreateCell(cell);
					else
						SkipCell();
				}
			}
		}

		protected void SkipCell() => new GameObject("BlankCell").transform.SetParent(layout.transform);
	}
}
