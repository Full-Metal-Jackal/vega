using UnityEngine;

namespace Circuitry
{
	public class DataInput<T> : DataPin<T>
	{
		public DataInput(Circuit circuit, string label) : base(circuit, label)
		{
		}
	}
}
