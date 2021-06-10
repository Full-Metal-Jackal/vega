using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI
{
	public class CircuitWidget : MonoBehaviour
	{
		public Circuit circuit;
		public bool Initialized { get; private set; } = false;

		public GameObject dataInputPrefab;
		public GameObject dataOutputPrefab;
		public GameObject pulseInputPrefab;
		public GameObject pulseOutputPrefab;

		private Transform inputsHolder;
		private Transform dataInputs;
		private Transform pulseInputs;
		private Transform outputsHolder;
		private Transform dataOutputs;
		private Transform pulseOutputs;

		private void Start()
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

			inputsHolder = transform.Find("Inputs");
			dataInputs = inputsHolder.Find("Data");
			pulseInputs = inputsHolder.Find("Pulse");

			outputsHolder = transform.Find("Outputs");
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

			return Initialized = true;
		}

		private PinWidget AddPin(GameObject widgetPrefab, Transform toParent, Pin pin)
		{
			GameObject widgetObject = Instantiate(widgetPrefab);
			widgetObject.transform.SetParent(toParent);

			PinWidget widget = widgetObject.GetComponent<PinWidget>();
			widget.SetLabel(pin.label);
			widget.pin = pin;

			return widget;
		}

		private void AddPin(DataInput input) => AddPin(dataInputPrefab, dataInputs, input);

		private void AddPin(DataOutput output) => AddPin(dataOutputPrefab, dataOutputs, output);

		private void AddPin(PulseInput input) => AddPin(pulseInputPrefab, pulseInputs, input);

		private void AddPin(PulseOutput output) => AddPin(pulseOutputPrefab, pulseOutputs, output);

		public override string ToString() => base.ToString() + $" (Holding {circuit})";
	}
}
