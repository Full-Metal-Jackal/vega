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
		public readonly string label;

		/// <summary>
		/// Description of the circuit shown in the constructor.
		/// </summary>
		public readonly string desc;

		/// <summary>
		/// The assembly this circuit is attached to.
		/// </summary>
		public Assembly assembly;

		/// <summary>
		/// List of circuit data inputs.
		/// </summary>
		private readonly List<DataInput<Data>> dataInputs = new List<DataInput<Data>>();

		/// <summary>
		/// List of circuit data outputs.
		/// </summary>
		private readonly List<DataOutput<Data>> dataOutputs = new List<DataOutput<Data>>();

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

		public Circuit(string label, string desc)
		{
			this.label = label;
			this.desc = desc;

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
		public DataInput<T> AddDataInput<T>(string label)
		{
			DataInput<T> pin = new DataInput<T>(this, label);
			dataInputs.Add(pin as DataInput<Data>);
			return pin;
		}

		/// <summary>
		/// Adds data output to the circuit.
		/// </summary>
		/// <typeparam name="T">Type of data. Must be derived from Circuitry.Data.</typeparam>
		/// <param name="label">The name of the output.</param>
		/// <returns>The added output.</returns>
		public DataOutput<T> AddDataOutput<T>(string label)
		{
			DataOutput<T> pin = new DataOutput<T>(this, label);
			dataOutputs.Add(pin as DataOutput<Data>);
			return pin;
		}

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
	}
}
