using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class PulseOutput : PulsePin
	{
		private readonly HashSet<PulseInput> destinations = new HashSet<PulseInput>();

		public PulseOutput(Circuit circuit, string label) : base(circuit, label)
		{
		}

		/// <summary>
		/// "Sends" pulse to every connected destination input.
		/// </summary>
		public override void Pulse()
		{
			foreach (PulseInput input in destinations)
				input.Pulse();
		}

		/// <summary>
		/// Adds destination input to the output.
		/// </summary>
		/// <param name="input">The destination input.</param>
		public void Connect(PulseInput input) => destinations.Add(input);

		/// <summary>
		/// Removes destination input from the output.
		/// </summary>
		/// <param name="input">The destination output.</param>
		public void Disconnect(PulseInput input) => destinations.Remove(input);
	}
}
