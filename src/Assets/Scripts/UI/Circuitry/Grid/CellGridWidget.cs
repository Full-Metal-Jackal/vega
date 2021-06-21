using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(GridLayoutGroup))]
	public class CellGridWidget : MonoBehaviour
	{
		public bool Initialized { get; protected set; } = false;

		[SerializeField]
		private GameObject cellPrefab;

		public GridLayoutGroup Layout { get; private set; }

		public RectTransform RectTransform { get; private set; }

		protected Circuitry.Shape shape;

		public Vector2 Size => Layout.cellSize * shape.Size;

		private void Awake()
		{
			Initialize();
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			Layout = GetComponent<GridLayoutGroup>();
			RectTransform = GetComponent<RectTransform>();

			return Initialized = true;
		}

		protected virtual CellWidget CreateCell(Vector2Int cell)
		{
			GameObject cellObject = Instantiate(cellPrefab);
			cellObject.transform.SetParent(Layout.transform, false);
			return cellObject.GetComponent<CellWidget>();
		}

		/// <summary>
		/// Removes every cell from the grid.
		/// </summary>
		public void ClearGrid()
		{
			foreach (Transform child in transform)
				Destroy(child.gameObject);
		}

		/// <summary>
		/// Creates grid for a provided shape.
		/// </summary>
		/// <param name="shape">Shape of the grid.</param>
		public void BuildGrid(Circuitry.Shape shape)
		{
			ClearGrid();

			(shape = shape.GetCopy()).Normalize();
			this.shape = shape;

			// Since unity is trash and we cannot directly place our stuff in proper cells,
			// we have to deal with the rectangular shape of the grid,
			// clogging the non-existing cells by calling the SkipCell method.
			Layout.constraintCount = shape.Width;
			for (int y = 0; y < shape.Height; y++)
			{
				for (int x = 0; x < shape.Width; x++)
				{
					Vector2Int cell = new Vector2Int(x, y);
					if (shape.HasCell(cell))
						CreateCell(cell);
					else
						SkipCell();
				}
			}
		}

		protected virtual void SkipCell() => new GameObject("BlankCell").transform.SetParent(Layout.transform);
	}
}
