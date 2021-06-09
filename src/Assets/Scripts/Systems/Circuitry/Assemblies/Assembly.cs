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

		private readonly HashSet<Circuit> circuits = new HashSet<Circuit>();

		public Assembly(string jsonSerializedAssembly)
		{
			throw new System.NotImplementedException();
		}

		public string JSONserialize()
		{
			throw new System.NotImplementedException();
		}

		public T AddCircuit<T>() where T : Circuit, new()
		{
			T circuit = new T();
			circuits.Add(circuit);
			return circuit;
		}
	}
}
