using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Pins are used to transfer data between circuits.
	/// </summary>
	public abstract class DataPin<T> : Element
	{
		public readonly string label;
		public T Value { get; private set; }

		protected DataPin(Circuit circuit, string label) : base(circuit)
		{
			if (!(Value is Data))
			{
				throw new System.Exception(
					"Invalid use of data pins, data pins should be set to Circuitry.Data derived types, "
					+ $"but {this} was set to {Value}");
			}
			this.label = label;
		}

		/// <summary>
		/// Directly sets the value stored in the pin.
		/// Doesn't affect anything else.
		/// </summary>
		/// <param name="value"></param>
		public void Set(T value) => Value = value;
	}
}
