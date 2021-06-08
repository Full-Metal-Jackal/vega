using UnityEngine;

namespace Circuitry
{
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
