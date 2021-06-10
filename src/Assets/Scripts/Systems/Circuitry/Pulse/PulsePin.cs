using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public abstract class PulsePin : Pin
	{
		public PulsePin(Circuit circuit, string label) : base(circuit, label)
		{
		}

		public virtual void Pulse()
		{
			UI.CircuitryLog.Log($"{circuit}: {this} has been pulsed.");
		}
	}
}
