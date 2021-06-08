using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public abstract class PulsePin : Element
	{
		public readonly string label;

		public PulsePin(Circuit circuit, string label) : base(circuit)
		{
			this.label = label;
		}

		public virtual void Pulse()
		{
		}
	}
}
