using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class CircuitWidget : MonoBehaviour, ITriggerable<Circuit>
	{
		public Circuit circuit;
		public bool Initialized { get; private set; } = false;

		public CircuitCooldownOverlay cooldownOverlay;
		public UIGridRenderer gridRenderer;

		public GameObject dataInputPrefab;
		public GameObject dataOutputPrefab;
		public GameObject pulseInputPrefab;
		public GameObject pulseOutputPrefab;

		private Transform dataInputs;
		private Transform pulseInputs;
		private Transform dataOutputs;
		private Transform pulseOutputs;

		private void Awake()
		{
			Initialize();
		}

		private bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			if (!circuit)
				throw new System.Exception("Attempted to create circuit panel without circuit.");

			if (!dataInputPrefab)
				throw new System.Exception($"Data input prefab not set up for {this}.");


			Transform pinsHolder = transform.Find("Pins");

			gridRenderer.AddCells(circuit.shape);

			Transform inputsHolder = pinsHolder.Find("Inputs");
			dataInputs = inputsHolder.Find("Data");
			pulseInputs = inputsHolder.Find("Pulse");

			Transform outputsHolder = pinsHolder.Find("Outputs");
			dataOutputs = outputsHolder.Find("Data");
			pulseOutputs = outputsHolder.Find("Pulse");

			foreach (DataInput input in circuit.GetDataInputs())
				AddPin(input);
			foreach (DataOutput output in circuit.GetDataOutputs())
				AddPin(output);
			foreach (PulseInput input in circuit.GetPulseInputs())
				AddPin(input);
			foreach (PulseOutput output in circuit.GetPulseOutputs())
				AddPin(output);

			EventHandler.Bind(this);

			return Initialized = true;
		}

		private PinWidget AddPin(GameObject widgetPrefab, Transform toParent, Pin pin)
		{
			GameObject widgetObject = Instantiate(widgetPrefab);
			widgetObject.transform.SetParent(toParent);

			PinWidget widget = widgetObject.GetComponentInChildren<PinWidget>();
			widget.Setup(pin);

			return widget;
		}

		private void AddPin(DataInput input) => AddPin(dataInputPrefab, dataInputs, input);

		private void AddPin(DataOutput output) => AddPin(dataOutputPrefab, dataOutputs, output);

		private void AddPin(PulseInput input) => AddPin(pulseInputPrefab, pulseInputs, input);

		private void AddPin(PulseOutput output) => AddPin(pulseOutputPrefab, pulseOutputs, output);

		public override string ToString() => base.ToString() + $" (Holding {circuit})";

		public void Cooldown()
		{
		}

		public bool Trigger(Circuit caller)
		{
			if (caller.IsSleeping)
				StartCooldownAnimation();

			return true;
		}

		public void StartCooldownAnimation()
		{
			cooldownOverlay.Activate(circuit.Cooldown);
		}
	}
}
