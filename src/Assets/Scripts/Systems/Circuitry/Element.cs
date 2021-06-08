using UnityEngine;

namespace Circuitry
{
	/// <summary>
	/// Represents elements attached to circuits: pins, terminals etc.
	/// </summary>
	public abstract class Element
	{
		/// <summary>
		/// The circuit this element belongs to.
		/// </summary>
		public readonly Circuit circuit;

		public Element(Circuit circuit)
		{
			this.circuit = circuit;
		}
	}
}
