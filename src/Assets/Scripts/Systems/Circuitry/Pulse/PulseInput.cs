using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Circuitry
{
	public class PulseInput : PulsePin
	{
		private readonly CircuitAction action;
		public readonly PulseOutput pipeline;

		public PulseInput(Circuit circuit, string label, CircuitAction action, PulseOutput pipeline = null) : base(circuit, label)
		{
			this.action = action;
			this.pipeline = pipeline;
		}

		public override void Pulse()
		{
			if (action())
				pipeline?.Pulse();
		}
	}
}
