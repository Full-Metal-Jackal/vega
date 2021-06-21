using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Pin : Element
	{
		public readonly string label;

		public Pin(Circuit circuit, string label) : base(circuit)
		{
			this.label = label;
		}

		public override string ToString() => label;
	}
}
