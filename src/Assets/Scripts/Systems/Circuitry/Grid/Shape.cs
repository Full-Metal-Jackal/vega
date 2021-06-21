using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public struct Shape
	{
		public readonly HashSet<Vector2Int> cells;

		public Shape(HashSet<Vector2Int> cells)
		{
			this.cells = cells;
		}

		public Shape(IEnumerable<Vector2Int> cells)
		{
			this.cells = new HashSet<Vector2Int>(cells);
		}

		public void AddCell(Vector2Int cell) => cells.Add(cell);
		public void AddCell(int x, int y) => AddCell(new Vector2Int(x, y));

		public Shape GetRotated()
		{
			HashSet<Vector2Int> rotated = new HashSet<Vector2Int>();

			return new Shape(rotated);
		}

		public Shape Normalized()
		{
			Vector2Int offset = new Vector2Int(int.MaxValue, int.MaxValue);
			foreach (Vector2Int cell in cells)
				offset = Vector2Int.Min(offset, cell);

			if (offset == Vector2Int.zero)
				return this;

			Shape normalized = Empty;
			foreach (Vector2Int cell in cells)
				normalized.AddCell(cell - offset);

			return normalized;
		}

		public static Shape Empty => new Shape(new HashSet<Vector2Int>());
		public static Shape Single => new Shape(new HashSet<Vector2Int>{ Vector2Int.zero });
		public static Shape Rect(int width, int height)
		{
			Shape shape = Empty;
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					shape.AddCell(x, y);
			return shape;
		}
	}
}
