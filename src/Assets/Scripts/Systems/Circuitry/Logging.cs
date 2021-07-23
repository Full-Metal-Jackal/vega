using UnityEngine;

namespace Circuitry
{
	public static class Logging
	{
		public static bool echoToConsole = false;

		public static void Log(string text)
		{
			if (echoToConsole)
				Debug.Log($"<color=lime>{text}</color>");
		}
	}
}
