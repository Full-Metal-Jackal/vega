using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class CircuitPorts : MonoBehaviour
	{
		[SerializeField]
		private PinWidget dataInputPrefab;

		[SerializeField]
		private PinWidget dataOutputPrefab;

		[SerializeField]
		private PinWidget pulseInputPrefab;

		[SerializeField]
		private PinWidget pulseOutputPrefab;

		protected readonly HashSet<PinWidget> pins = new HashSet<PinWidget>();

		public void Setup(CircuitContainer circuit)
		{
			// кайкоц краирсирсый окдок прияптнонго аетпаиатаа!!
			foreach (DataInput input in circuit.Circuit.GetDataInputs())
				pins.Add(circuit.AddPin(dataInputPrefab, input));
			foreach (DataOutput output in circuit.Circuit.GetDataOutputs())
				pins.Add(circuit.AddPin(dataOutputPrefab, output));
			foreach (PulseInput input in circuit.Circuit.GetPulseInputs())
				pins.Add(circuit.AddPin(pulseInputPrefab, input));
			foreach (PulseOutput output in circuit.Circuit.GetPulseOutputs())
				pins.Add(circuit.AddPin(pulseOutputPrefab, output));
		}

		public IEnumerable<PinWidget> Pins => new HashSet<PinWidget>(pins);
	}
}
