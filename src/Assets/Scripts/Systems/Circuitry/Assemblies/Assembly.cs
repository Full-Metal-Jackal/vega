using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Represents an assembly of circuits.
	/// </summary>
	public abstract class Assembly : MonoBehaviour
	{
		public string label = "unnamed assembly";

		public Assembly(string jsonSerializedAssembly)
		{
			// <TODO>
		}

		public string JSONserialize()
		{
			throw new System.NotImplementedException();
		}
	}
}
