﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public delegate bool CircuitAction();

	/// <summary>
	/// Represents a circuit in assembly.
	/// 
	/// Ideally should be completely devoid of MonoBehaviour inheritance.
	/// </summary>
	public class Circuit : MonoBehaviour
	{
		public bool Initialized { get; set; } = false;

		[SerializeField]
		private string label;
		public string Label => label;

		[SerializeField]
		private string desc;
		public string Desc => desc;

		public Category category;

		/// <summary>
		/// The assembly this circuit is attached to.
		/// </summary>
		protected Assembly assembly;

		/// <summary>
		/// The grid cells occupied by this circuit.
		/// </summary>
		public Shape Shape { get; protected set; }

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
		public bool IsSleeping => (Cooldown > 0f);

		/// <summary>
		/// How much seconds left to the circuit's awakening.
		/// </summary>
		public float Cooldown { get; protected set; } = 0f;

		/// <summary>
		/// How much power is withdrawn from the assembly per single use.
		/// This value is shown in the circuit info tooltip, but may vary in the circuit implementation.
		/// </summary>
		public virtual float PowerConsumption => 10f;

		/// <summary>
		/// How much time should pass between two uses of this circuit.
		/// This value is shown in the circuit info tooltip, but may vary in the circuit implementation.
		/// </summary>
		public virtual float CooldownPerUse => .1f;

		/// <summary>
		/// Makes the circuit inactive for the set amount of time.
		/// An inactive circuit is unable to receive and send pulses.
		/// </summary>
		/// <param name="time">Amount of seconds to sleep.</param>
		public void Sleep(float time)
		{
			Cooldown = time;
			UI.CircuitConstructor.EventHandler.Trigger(this, "cooldown");
		}

		private void Awake()
		{
			Initialize();
			Setup();
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			Shape = Shape.Single;
			PowerInput = new InputTerminal(this);

			return Initialized = true;
		}

		public virtual void Setup()
		{
		}

		private void FixedUpdate()
		{
			if (IsSleeping)
				Cooldown = Math.Max(Cooldown - Time.fixedDeltaTime, 0f);
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
		public IEnumerable<DataInput> GetDataInputs() => new List<DataInput>(dataInputs);

		/// <summary>
		/// Gets attached data output pins.
		/// </summary>
		/// <returns>Attached data output pins.</returns>
		public IEnumerable<DataOutput> GetDataOutputs() => new List<DataOutput>(dataOutputs);

		/// <summary>
		/// Gets attached pulse input pins.
		/// </summary>
		/// <returns>Attached pulse input pins.</returns>
		public IEnumerable<PulseInput> GetPulseInputs() => new List<PulseInput>(pulseInputs);

		/// <summary>
		/// Gets attached pulse output pins.
		/// </summary>
		/// <returns>Attached pulse output pins.</returns>
		public IEnumerable<PulseOutput> GetPulseOutputs() => new List<PulseOutput>(pulseOutputs);

		public override string ToString() => label;
	}
}
