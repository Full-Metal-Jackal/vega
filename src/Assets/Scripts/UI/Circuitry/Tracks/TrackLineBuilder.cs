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
	[RequireComponent(typeof(RectTransform))]
	public class TrackLineBuilder : MonoBehaviour
	{
		public bool Initialized { get; protected set; } = false;

		private UILineRenderer line;

		public PinWidget startPin;
		public PinWidget endPin;

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

			line = GetComponent<UILineRenderer>();
			CreateLine();

			return Initialized = true;
		}

		/// <summary>
		/// Creates a new line bound to a pair of pins.
		/// </summary>
		/// <param name="start">The start point of the line.</param>
		/// <param name="end">The end point of the line.</param>
		public void CreateLine(PinWidget start, PinWidget end)
		{
			startPin = start;
			endPin = end;

			CreateLine(
				Vector2.zero,
				end.transform.position - start.transform.position
				);
		}

		/// <summary>
		/// Creates a new unbound line.
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
		/// Updates line according to bound pins.
		/// </summary>
		public void UpdateLine()
		{
			if (!(startPin && endPin))
				return;

			UpdateLine(
				Vector2.zero,
				ScreenToPinLocal(startPin, endPin.ButtonRectTransform.position)
				);
		}

		/// <summary>
		/// Updates the start and the end vertices of the line.
		/// </summary>
		/// <param name="start">The start screenspace position.</param>
		/// <param name="end">The end screenspace position.</param>
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

		public Vector2 ScreenToPinLocal(PinWidget pin, Vector2 position)
		{
			position -= (Vector2)pin.ButtonRectTransform.position;
			position /= CircuitConstructor.Instance.GridScale;
			return position;
		}
	}
}
