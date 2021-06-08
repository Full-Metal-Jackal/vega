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
			throw new System.NotImplementedException();
		}

		public string JSONserialize()
		{
			throw new System.NotImplementedException();
		}
	}
}
