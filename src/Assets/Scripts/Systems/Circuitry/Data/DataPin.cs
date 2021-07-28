using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Pins are used to transfer data between circuits.
	/// </summary>
	public abstract class DataPin : Pin
	{
		public delegate void ValueAction(Data value);
		public event ValueAction OnValueChanged;

		public Data Value { get; private set; }

		protected DataPin(Circuit circuit, string label) : base(circuit, label)
		{
		}

		/// <summary>
		/// Directly sets the value stored in the pin.
		/// Doesn't affect anything else.
		/// </summary>
		/// <param name="value"></param>
		public void Set(Data value)
		{
			Value = value;
			Logging.Log($"{circuit}: {label} has been set to {value}");
			OnValueChanged?.Invoke(value);
		}
	}
}
