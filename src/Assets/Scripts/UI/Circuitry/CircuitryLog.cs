using UnityEngine;

using Circuitry;

namespace UI
{
	public static class CircuitryLog
	{
		public static bool echoToConsole = true;

		public static void Log(string text)
		{
			if (echoToConsole)
				Debug.Log($"<color=lime>[{0}] {text}</color>");
		}
	}
}
