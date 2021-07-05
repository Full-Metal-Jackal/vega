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
}
