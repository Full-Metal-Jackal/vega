using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class CircuitPorts : MonoBehaviour
	{
		[SerializeField]
		private GameObject dataInputPrefab;

		[SerializeField]
		private GameObject dataOutputPrefab;

		[SerializeField]
		private GameObject pulseInputPrefab;

		[SerializeField]
		private GameObject pulseOutputPrefab;

		protected readonly HashSet<PinWidgetBase> pins = new HashSet<PinWidgetBase>();

		public void Setup(Circuit circuit)
		{
			// кайкоц краирсирсый окдок прияптнонго аетпаиатаа!!
			foreach (DataInput input in circuit.BoundCircuit.GetDataInputs())
				pins.Add(circuit.AddPin(dataInputPrefab, input));
			foreach (DataOutput output in circuit.BoundCircuit.GetDataOutputs())
				pins.Add(circuit.AddPin(dataOutputPrefab, output));
			foreach (PulseInput input in circuit.BoundCircuit.GetPulseInputs())
				pins.Add(circuit.AddPin(pulseInputPrefab, input));
			foreach (PulseOutput output in circuit.BoundCircuit.GetPulseOutputs())
				pins.Add(circuit.AddPin(pulseOutputPrefab, output));
		}

		public IEnumerable<PinWidgetBase> Pins => new HashSet<PinWidgetBase>(pins);
	}
}
