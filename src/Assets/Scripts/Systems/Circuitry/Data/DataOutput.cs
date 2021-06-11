using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class DataOutput : DataPin
	{
		private readonly HashSet<DataInput> destinations = new HashSet<DataInput>();

		public DataOutput(Circuit circuit, string label) : base(circuit, label)
		{
		}

		/// <summary>
		/// Sets the value and pushes it to destination pins.
		/// Intended to be used by circuits.
		/// </summary>
		/// <param name="value"></param>
		public void Push(Data value)
		{
			Set(value);
			Push();
		}

		/// <summary>
		/// Sets the value of every destination pin to this pin's value.
		/// </summary>
		public void Push()
		{
			foreach (DataInput input in destinations)
				input.Set(Value);
		}

		/// <summary>
		/// Adds destination input pin to this output.
		/// </summary>
		/// <param name="input"></param>
		public bool Connect(DataInput input)
		{
			if (!destinations.Add(input))
				return false;
			UI.CircuitryLog.Log($"{this} of {circuit} has been connected to {input} of {input.circuit}");
			return true;
		}
	}
}
