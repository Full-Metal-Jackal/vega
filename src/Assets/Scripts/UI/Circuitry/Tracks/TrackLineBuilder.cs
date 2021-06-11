using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	/// <summary>
	/// Class responsible for building a line that looks like a circuit track and does not intersect with 
	/// </summary>
	[RequireComponent(typeof(UILineRenderer))]
	public class TrackLineBuilder : MonoBehaviour
	{
		private UILineRenderer line;

		private void Awake()
		{
			line = GetComponent<UILineRenderer>();
			CreateLine();
		}

		/// <summary>
		/// Creates a new line.
		/// </summary>
		/// <param name="start">The start point of the line.</param>
		/// <param name="end">The end point of the line.</param>
		public void CreateLine(Vector2 start, Vector2 end)
		{
			while (line.dots.Count < 2)
				line.dots.Add(Vector2.zero);

			UpdateLine(start, end);
		}
		public void CreateLine() => CreateLine(Vector2.zero, Vector2.zero);

		/// <summary>
		/// Updates the line to be shaped as a circuit track.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		public void UpdateLine(Vector2 start, Vector2 end)
		{
			UpdateLineStart(start);
			UpdateLineEnd(end);
		}

		public void UpdateLineStart(Vector2 start)
		{
			if (line.dots.Count < 1)
				return;

			line.dots[0] = start;
			line.SetVerticesDirty();
		}

		public void UpdateLineEnd(Vector2 end)
		{
			if (line.dots.Count < 1)
				return;

			line.dots[line.dots.Count - 1] = end;
			line.SetVerticesDirty();
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}
