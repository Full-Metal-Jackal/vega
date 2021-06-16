using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public struct Shape
	{
		private readonly HashSet<Vector2Int> cells;
		public HashSet<Vector2Int> Cells => new HashSet<Vector2Int>(cells);
		public int CellCount => Cells.Count;
		public int Width { get; private set; }
		public int Height { get; private set; }
		public Vector2Int Size
		{
			get => new Vector2Int(Width, Height);
			private set
			{
				Width = value.x;
				Height = value.y;
			}
		}

		/// <summary>
		/// A vector composed of minimal x and y coordinates in the shape.
		/// </summary>
		public Vector2Int Origin { get; private set; }

		public Shape(IEnumerable<Vector2Int> cells)
		{
			this.cells = new HashSet<Vector2Int>(cells);
			Width = 0;
			Height = 0;
			Origin = Vector2Int.zero;
			Origin = GetOrigin();
			Size = GetSize();
		}

		public void AddCell(Vector2Int cell)
		{
			Origin = Vector2Int.Min(Origin, cell);
			Size = Vector2Int.Max(Size, cell);
			cells.Add(cell);
		}

		public void AddCell(int x, int y) => AddCell(new Vector2Int(x, y));

		public Vector2Int GetSize()
		{
			Vector2Int size = new Vector2Int(int.MinValue, int.MinValue);
			foreach (Vector2Int cell in cells)
				size = Vector2Int.Max(size, cell);
			return size - GetOrigin() + Vector2Int.one;
		}

		public Vector2Int GetOrigin()
		{
			Vector2Int origin = new Vector2Int(int.MaxValue, int.MaxValue);
			foreach (Vector2Int cell in cells)
				origin = Vector2Int.Min(origin, cell);
			return origin;
		}

		public Shape GetRotated()
		{
			throw new System.NotImplementedException();
		}

		public Shape Center()
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Shifts the shape so that its origin is zero.
		/// </summary>
		public void Normalize()
		{
			Vector2Int origin = GetOrigin();

			if (origin == Vector2Int.zero)
				return;

			HashSet<Vector2Int> normalizedShape = new HashSet<Vector2Int>();
			foreach (Vector2Int cell in cells)
				normalizedShape.Add(cell - origin);

			cells.Clear();
			cells.UnionWith(normalizedShape);

			Origin = Vector2Int.zero;
		}
	
		public bool HasCell(Vector2Int cell) => cells.Contains(cell);

		public Shape GetCopy() => new Shape(Cells);

		public void Join(Shape shape, Vector2Int offset)
		{
			foreach (Vector2Int cell in shape.Cells)
				shape.AddCell(cell + offset);
		}

		public void Join(Shape shape) => Join(shape, Vector2Int.zero);

		public void AddRect(int width, int height, Vector2Int position)
		{
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					AddCell(new Vector2Int(x, y) + position);
		}

		public static Shape Empty => new Shape(new HashSet<Vector2Int>());
		public static Shape Single => new Shape(new HashSet<Vector2Int>{ Vector2Int.zero });
		public static Shape Rect(int width, int height)
		{
			Shape shape = Empty;
			shape.AddRect(width, height, Vector2Int.zero);
			return shape;
		}
	}
}
