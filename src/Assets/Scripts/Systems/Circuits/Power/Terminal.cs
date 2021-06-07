using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Terminal are used to transfer pulses and power between circuit.
	/// </summary>
	public class Terminal : MonoBehaviour
	{
		/// <summary>
		/// The name of the terminal.
		/// </summary>
		public string label;

		/// <summary>
		/// The amperage limit for the terminal.
		/// </summary>
		public float maxAmperage = 1f;
	}
}
