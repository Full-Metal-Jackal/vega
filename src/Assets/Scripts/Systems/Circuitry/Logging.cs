using UnityEngine;

namespace Circuitry
{
	public static class Logging
	{
		public static void Log(string text) => Debug.Log($"<color=green>{text}</color>");
	}
}
