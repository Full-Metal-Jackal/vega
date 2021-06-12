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

		/// <summary>
		/// Performs the assigned action.
		/// If the input is a part of pipeline, the pipeline's output will be pulsed after the action is performed.
		/// </summary>
		public override void Pulse()
		{
			if (circuit.IsSleeping)
				return;

			if (action())
				pipeline?.Pulse();
			
			base.Pulse();
		}
	}
}
