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

		public override void Pulse()
		{
			foreach (PulseInput input in destinations)
				input.Pulse();
		}

		public void AddDestination(PulseInput input) => destinations.Add(input);
		public void RemoveDestination(PulseInput input) => destinations.Remove(input);
	}
}
