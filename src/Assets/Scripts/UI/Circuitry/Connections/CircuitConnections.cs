using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class CircuitConnections : MonoBehaviour
	{
		[SerializeField]
		private GameObject dataInputPrefab;

		[SerializeField]
		private GameObject dataOutputPrefab;

		[SerializeField]
		private GameObject pulseInputPrefab;

		[SerializeField]
		private GameObject pulseOutputPrefab;

		public void Setup(Circuit circuit)
		{
			// кайкоц краирсирсый окдок прияптнонго аетпаиатаа!!
			foreach (DataInput input in circuit.BoundCircuit.GetDataInputs())
				circuit.AddPin(dataInputPrefab, input);
			foreach (DataOutput output in circuit.BoundCircuit.GetDataOutputs())
				circuit.AddPin(dataOutputPrefab, output);
			foreach (PulseInput input in circuit.BoundCircuit.GetPulseInputs())
				circuit.AddPin(pulseInputPrefab, input);
			foreach (PulseOutput output in circuit.BoundCircuit.GetPulseOutputs())
				circuit.AddPin(pulseOutputPrefab, output);
		}
	}
}
