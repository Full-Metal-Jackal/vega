using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UI;

namespace UI.CircuitConstructor
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
		public void ReshapeLine()
		{
			line.SetVerticesDirty();
		}

		/// <summary>
		/// Updates the start and the end vertices of the line.
		/// </summary>
		/// <param name="start">The start position.</param>
		/// <param name="end">The end position.</param>
		public void UpdateLine(Vector2 start, Vector2 end)
		{
			if (line.dots.Count < 2)
				return;

			line.dots[0] = start;
			line.dots[line.dots.Count - 1] = end;
			ReshapeLine();
		}

		public void UpdateLineStart(Vector2 start)
		{
			if (line.dots.Count < 1)
				return;

			line.dots[0] = start;
			ReshapeLine();
		}

		public void UpdateLineEnd(Vector2 end)
		{
			if (line.dots.Count < 1)
				return;

			line.dots[line.dots.Count - 1] = end;
			line.SetVerticesDirty();
			ReshapeLine();
		}

		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}
