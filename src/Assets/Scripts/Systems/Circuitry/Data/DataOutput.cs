using UnityEngine;

namespace Circuitry
{
	public class DataOutput<T> : DataPin<T>
	{
		public DataInput<Data> destination;

		public DataOutput(Circuit circuit, string label) : base(circuit, label)
		{
		}

		/// <summary>
		/// Set the value and push it to the destination pin.
		/// Intended to be used by circuits.
		/// </summary>
		/// <param name="value"></param>
		public void Push(T value)
		{
			Set(value);
			Push();
		}
		/// <summary>
		/// Sets the value of the destination pin to this pin's value.
		/// The essential element in the data transfering system.
		/// </summary>
		public void Push() => destination?.Set(Value as Data);
	}
}
