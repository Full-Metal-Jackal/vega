using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Represents an assembly of circuits.
	/// </summary>
	public abstract class Assembly : MonoBehaviour
	{
		public string label = "unnamed assembly";
		public bool Initialized { get; set; } = false;

		private readonly HashSet<Circuit> circuits = new HashSet<Circuit>();

		public readonly AssemblyGrid grid = new AssemblyGrid();

		private void Awake()
		{
			Initialize();
		}

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			// <TODO> This call should be moved to the Start message as soon as we implement assemblies initializtion in game;
			// It should stay here till we initialize our widgets simultaneously with assemblies.
			Setup();

			return Initialized = true;
		}

		public abstract void Setup();

		/// <summary>
		/// Attempts to deserialize the assembly from a string.
		/// </summary>
		/// <param name="serializedAssembly"></param>
		/// <returns>The deserialized assembly in case of successful deserialization, null otherwise.</returns>
		public static Assembly Deserialize(string serializedAssembly)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Serializes the assembly, allowing to store in a string.
		/// </summary>
		/// <returns>JSON-serialized string that represents the assembly.</returns>
		public string Serialize()
		{
			throw new System.NotImplementedException();
		}

		public T AddCircuit<T>(Vector2Int cell) where T : Circuit, new()
		{
			T circuit = new T();
			circuits.Add(circuit);
			AddCircuit(circuit, cell);
			return circuit;
		}

		public void AddCircuit(Circuit circuit, Vector2Int cell)
		{
			if (!grid.AddCircuit(circuit, cell))
				return;
			circuits.Add(circuit);
			UI.CircuitConstructor.EventHandler.Log($"{this}: {circuit} has been placed at {cell}");
		}
	}
}
