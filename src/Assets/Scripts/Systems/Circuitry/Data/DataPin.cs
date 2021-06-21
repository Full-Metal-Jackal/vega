﻿using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Pins are used to transfer data between circuits.
	/// </summary>
	public abstract class DataPin : Pin
	{
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
			UI.CircuitConstructor.EventHandler.Log($"{circuit}: {label} has been set to {value}");
			UI.CircuitConstructor.EventHandler.Trigger(this);
		}
	}
}
