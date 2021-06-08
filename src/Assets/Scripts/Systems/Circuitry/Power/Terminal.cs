using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Terminal are used to transfer power between circuit.
	/// </summary>
	public abstract class Terminal : Element
	{
		/// <summary>
		/// The amperage limit for the terminal.
		/// </summary>
		public float maxAmperage = 1f;

		protected Terminal(Circuit circuit) : base(circuit)
		{
		}
	}
}
