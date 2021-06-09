using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI
{
	public class CircuitPanel : MonoBehaviour
	{
		public Circuit circuit;
		public bool Initialized { get; private set; } = false;

		public GameObject dataInputPrefab;

		private void Start()
		{
			if (!circuit)
				return;

			foreach (DataInput<Data> input in circuit.GetDataInputs())
				AddDataInput(input);
		}

		private void AddDataInput(DataInput<Data> input)
		{

		}
	}
}
