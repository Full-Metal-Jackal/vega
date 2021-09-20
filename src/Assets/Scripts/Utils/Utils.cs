using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	/// <summary>
	/// Omits y components of two points and returns distance between them.
	/// </summary>
	/// <param name="source">Source point.</param>
	/// <param name="target">Destination point.</param>
	/// <returns>Horizontal distance.</returns>
	public static float HorizontalDistance(Vector3 source, Vector3 target)
	{
		target -= source;
		target.y = 0;
		return target.magnitude;
	}

	// Спиздил и немного рефакторнул.
	/// <summary>
	/// Generates a smoothed bezier curve from the provided line.
	/// </summary>
	/// <param name="line">The original line represented by an array of points.</param>
	/// <param name="smoothness">The smoothness of the line.
	/// Mostly represents the multiplication of the original line dots.
	/// Must be over 0.</param>
	/// <returns>Array of points the smoothed line consists of.</returns>
	public static Vector3[] SmoothLine(Vector3[] line, float smoothness)
	{
		List<Vector3> points;
		List<Vector3> smoothedLine;

		if (smoothness < 1f)
			throw new System.Exception("Smoothness parameter is not supposed to be less than 1.");

		int pointsLength = line.Length;

		int curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
		smoothedLine = new List<Vector3>(curvedLength);

		for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
		{
			float t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

			points = new List<Vector3>(line);

			for (int j = pointsLength - 1; j > 0; j--)
				for (int i = 0; i < j; i++)
					points[i] = (1 - t) * points[i] + t * points[i + 1];

			smoothedLine.Add(points[0]);
		}

		return smoothedLine.ToArray();
	}

	/// <summary>
	/// Shortcut for quick picking a random item from a list.
	/// </summary>
	/// <typeparam name="T">The list's type.</typeparam>
	/// <param name="list">The list to pick from.</param>
	/// <returns>A random element from the list.</returns>
	public static T Pick<T>(List<T> list) => list[Random.Range(0, list.Count)];
}
