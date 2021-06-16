using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(Circuit))]
	public class CircuitWidget : MonoBehaviour, ITriggerable<Circuit>
	{
		public Circuit BoundCircuit { get; private set; }
		public bool Initialized { get; private set; } = false;

		public CircuitCooldownOverlay cooldownOverlay;
		public ConstructorGrid grid;

		[SerializeField]
		private GameObject dataInputPrefab;
		[SerializeField]
		private GameObject dataOutputPrefab;
		[SerializeField]
		private GameObject pulseInputPrefab;
		[SerializeField]
		private GameObject pulseOutputPrefab;

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

			if (!(dataInputPrefab && dataOutputPrefab && pulseInputPrefab && pulseOutputPrefab))
				throw new System.Exception($"Pin prefabs not set up properly for {this}.");

			BoundCircuit = GetComponent<Circuit>();

			Transform pinsHolder = transform.Find("Pins");

			Transform inputsHolder = pinsHolder.Find("Inputs");
			dataInputs = inputsHolder.Find("Data");
			pulseInputs = inputsHolder.Find("Pulse");

			Transform outputsHolder = pinsHolder.Find("Outputs");
			dataOutputs = outputsHolder.Find("Data");
			pulseOutputs = outputsHolder.Find("Pulse");

			EventHandler.Bind(this);

			return Initialized = true;
		}

		private void Start()
		{
			Setup();

			foreach (DataInput input in BoundCircuit.GetDataInputs())
				AddPin(input);
			foreach (DataOutput output in BoundCircuit.GetDataOutputs())
				AddPin(output);
			foreach (PulseInput input in BoundCircuit.GetPulseInputs())
				AddPin(input);
			foreach (PulseOutput output in BoundCircuit.GetPulseOutputs())
				AddPin(output);
		}

		public void Setup()
		{
			grid.BuildGrid(BoundCircuit.shape);
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

		public override string ToString() => $"{BoundCircuit}'s widget";

		public bool Trigger(Circuit caller)
		{
			if (caller.IsSleeping)
				StartCooldownAnimation();

			return true;
		}

		public void StartCooldownAnimation()
		{
			cooldownOverlay.Activate(BoundCircuit.Cooldown);
		}
	}
}
