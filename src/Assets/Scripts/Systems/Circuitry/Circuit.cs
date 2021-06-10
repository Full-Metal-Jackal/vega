using System;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public delegate bool CircuitAction();

	/// <summary>
	/// Represents a circuit.
	/// </summary>
	public abstract class Circuit : MonoBehaviour
	{
		/// <summary>
		/// The name of the circuit.
		/// </summary>
		public string label;

		/// <summary>
		/// Description of the circuit shown in the constructor.
		/// </summary>
		public string desc;

		/// <summary>
		/// The assembly this circuit is attached to.
		/// </summary>
		public Assembly assembly;

		/// <summary>
		/// List of circuit data inputs.
		/// </summary>
		private readonly List<DataInput> dataInputs = new List<DataInput>();

		/// <summary>
		/// List of circuit data outputs.
		/// </summary>
		private readonly List<DataOutput> dataOutputs = new List<DataOutput>();

		/// <summary>
		/// List of circuit data inputs.
		/// </summary>
		private readonly List<PulseInput> pulseInputs = new List<PulseInput>();

		/// <summary>
		/// List of circuit data outputs.
		/// </summary>
		private readonly List<PulseOutput> pulseOutputs = new List<PulseOutput>();

		/// <summary>
		/// The power input of the circuit.
		/// </summary>
		public InputTerminal PowerInput { get; private set; }

		/// <summary>
		/// If the circuit is currently active (not affected by the Sleep method).
		/// </summary>
		public bool IsSleeping => (cooldown > 0f);
		protected float cooldown = 0f;

		/// <summary>
		/// How much power is withdrawn from the assembly per single use.
		/// </summary>
		public virtual float PowerConsumption => 10f;

		public Circuit()
		{
			PowerInput = new InputTerminal(this);
		}

		/// <summary>
		/// Makes the circuit inactive for the set amount of time.
		/// An inactive circuit is unable to receive and send pulses.
		/// </summary>
		/// <param name="time">Amount of seconds to sleep.</param>
		public void Sleep(float time)
		{
			cooldown = time;
		}

		private void FixedUpdate()
		{
			if (IsSleeping)
				cooldown = Math.Max(cooldown - Time.fixedDeltaTime, 0f);
		}

		/// <summary>
		/// Withdraws power from the power source.
		/// </summary>
		/// <param name="amount">Amount of power to withdraw.</param>
		/// <returns>Amount of power actually withdrawn.</returns>
		public float UsePower(float amount) => PowerInput.Withdraw(amount);

		/// <summary>
		/// Withdraws power from the power source, uses PowerConsumption value.
		/// </summary>
		/// <returns>Fraction of succesfully withdrawn power.</returns>
		public float UsePower() => PowerInput.Withdraw(PowerConsumption) / PowerConsumption;

		/// <summary>
		/// Withdraws power from the power source, uses PowerConsumption value.
		/// </summary>
		/// <returns>True if all the requested amount has been withdrawn, false otherwise.</returns>
		public bool UseFullPower() => PowerInput.Withdraw(PowerConsumption) >= PowerConsumption;

		/// <summary>
		/// Adds data input to the circuit.
		/// </summary>
		/// <typeparam name="T">Type of data. Must be derived from Circuitry.Data.</typeparam>
		/// <param name="label">The name of the input.</param>
		/// <returns>The added input.</returns>
		public DataInput AddDataInput<T>(string label) where T : Data, new()
		{
			DataInput pin = new DataInput(this, label);
			pin.Set(new T());

			dataInputs.Add(pin);
			return pin;
		}

		public DataInput AddDataInput(string label) => AddDataInput<Null>(label);

		/// <summary>
		/// Adds data output to the circuit.
		/// </summary>
		/// <typeparam name="T">Type of data. Must be derived from Circuitry.Data.</typeparam>
		/// <param name="label">The name of the output.</param>
		/// <returns>The added output.</returns>
		public DataOutput AddDataOutput<T>(string label) where T : Data, new()
		{
			DataOutput pin = new DataOutput(this, label);
			pin.Set(new T());

			dataOutputs.Add(pin);
			return pin;
		}
		public DataOutput AddDataOutput(string label) => AddDataOutput<Null>(label);

		private PulseInput AddPulseInput(string label, CircuitAction action, PulseOutput pipeline = null)
		{
			PulseInput input = new PulseInput(this, label, action, pipeline);
			pulseInputs.Add(input);
			return input;
		}

		/// <summary>
		/// Adds pulse input to the circuit.
		/// </summary>
		/// <param name="label">The name of the input.</param>
		/// <param name="action">The action performed by the circuit when the input is pulsed.</param>
		/// <returns>The added input.</returns>
		public PulseInput AddPulseInput(string label, CircuitAction action) => AddPulseInput(label, action);

		/// <summary>
		/// Adds pulse output to the circuit.
		/// </summary>
		/// <param name="label">The name of the output.</param>
		/// <returns>The added output.</returns>
		public PulseOutput AddPulseOutput(string label)
		{
			PulseOutput output = new PulseOutput(this, label);
			pulseOutputs.Add(output);
			return output;
		}

		/// <summary>
		/// Adds pipeline for primitive circuits, consisting of input and output pulse pins.
		/// After the provided action is complete, the output is automatically pulsed.
		/// </summary>
		/// <param name="inputLabel">The name of the input.</param>
		/// <param name="action">The action performed by the circuit when the input is pulsed.</param>
		/// <param name="outputLabel">The name of the output.</param>
		public void AddPulsePipeline(string inputLabel, CircuitAction action, string outputLabel)
		{
			AddPulseInput(inputLabel, action, AddPulseOutput(outputLabel));
		}

		/// <summary>
		/// Gets attached data input pins.
		/// </summary>
		/// <returns>Attached data input pins.</returns>
		public IEnumerable<DataInput> GetDataInputs()
		{
			foreach (DataInput input in dataInputs)
				yield return input;
		}

		/// <summary>
		/// Gets attached data output pins.
		/// </summary>
		/// <returns>Attached data output pins.</returns>
		public IEnumerable<DataOutput> GetDataOutputs()
		{
			foreach (DataOutput output in dataOutputs)
				yield return output;
		}

		/// <summary>
		/// Gets attached pulse input pins.
		/// </summary>
		/// <returns>Attached pulse input pins.</returns>
		public IEnumerable<PulseInput> GetPulseInputs()
		{
			foreach (PulseInput input in pulseInputs)
				yield return input;
		}

		/// <summary>
		/// Gets attached pulse output pins.
		/// </summary>
		/// <returns>Attached pulse output pins.</returns>
		public IEnumerable<PulseOutput> GetPulseOutputs()
		{
			foreach (PulseOutput output in pulseOutputs)
				yield return output;
		}

		public override string ToString() => label;
	}
}
