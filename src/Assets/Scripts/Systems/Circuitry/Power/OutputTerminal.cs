using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Output terminals are used to output power from various power sources such as power cell and capacitors.
	/// </summary>
	public class OutputTerminal : Terminal
	{
		public OutputTerminal(Circuit circuit) : base(circuit)
		{
			if (circuit is IPowerSource powerSource)
				this.powerSource = powerSource;
			else
				throw new System.Exception("Attempting to attach a power output to a not power source circuit.");
		}

		private IPowerSource powerSource;

		/// <summary>
		/// Attempts to withdraw power from the circuit.
		/// </summary>
		/// <param name="amount">Amount of power to withdraw.</param>
		/// <returns>Amount of the withdrawn power.</returns>
		public float Withdraw(float amount) => powerSource.Withdraw(amount);
	}
}
