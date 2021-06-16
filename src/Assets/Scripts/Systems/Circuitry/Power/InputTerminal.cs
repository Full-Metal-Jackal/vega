using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Input terminals are used to supply circuits they are installed on with power.
	/// </summary>
	public class InputTerminal : Terminal
	{
		private OutputTerminal source;

		public InputTerminal(Circuit circuit) : base(circuit)
		{
		}

		/// <summary>
		/// Attempts to withdraw power from the connected outputTerminal or directly from the battery.
		/// </summary>
		/// <param name="amount">Amount of power to withdraw.</param>
		/// <returns>Amount of the withdrawn power.</returns>
		public float Withdraw(float amount)
		{
			float withdrawn = 0f;
			if (source is null)
			{
				// <TODO> Remove when the power system will be implemented.
				return amount;
			}
			else
			{
				withdrawn = source.Withdraw(amount);
			}
			return withdrawn;
		}

		/// <summary>
		/// Connects two terminals.
		/// </summary>
		/// <param name="output">The terimal from which this one will draw power.</param>
		public void Connect(OutputTerminal output)
		{
			source = output;
		}
	}
}
