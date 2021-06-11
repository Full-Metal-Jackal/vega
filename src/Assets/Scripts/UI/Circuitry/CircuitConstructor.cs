using UnityEngine;

using Circuitry;

namespace UI
{
	public class CircuitConstructor : MonoBehaviour
	{

		public GameObject circuitPrefab;

		public Assembly assembly;

		public bool Initialized { get; private set; }

		private void Start()
		{
			Initialize();
		}

		private bool Initialize()
		{
			if (Game.circuitConstructor)
				throw new System.Exception($"Multiple instances of circuit constructor detected: {this}, {Game.circuitConstructor}");
			Game.circuitConstructor = this;

			gameObject.SetActive(false);

			if (!circuitPrefab)
				throw new System.Exception($"No circuit prefab provided to the circuit constructor.");

			return Initialized = true;
		}

		public void Open()
		{
			Debug.Log("Opening the circuit constructor...");
			gameObject.SetActive(true);
			Game.inputState = InputState.UIOnly;
		}

		public void Close()
		{
			Debug.Log("Closing the circuit constructor...");
			gameObject.SetActive(false);
			Game.inputState = InputState.WorldOnly;
		}

		public void ShowCircuitInfo(Circuit circuit)
		{
			Debug.Log($"Information about {circuit} should be shown now.");
		}

		public void SpawnCircuit<T>() where T : Circuit, new()
		{
			GameObject uiCircuit = Instantiate(circuitPrefab);
			CircuitWidget panel = uiCircuit.GetComponent<CircuitWidget>();
		}
	}
}
