using UnityEngine;

namespace Circuitry
{
	public class DataInput : DataPin
	{
		public DataInput(Circuit circuit, string label) : base(circuit, label)
		{
		}

		public override string ToString() => $"{this} [{Value}]";
	}
}
