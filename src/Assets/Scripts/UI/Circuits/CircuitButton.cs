using UnityEngine;
using UnityEngine.UI;

using Circuitry;

namespace UI
{
	public class CircuitButton : MonoBehaviour
	{
		public Circuit circuit;
		public Image image;

		private void Start()
		{
			image = GetComponentInChildren<Image>();
			// image.sprite = circuit.<TODO implement the circuit info field>
		}

		public CircuitButton(Circuit circuit)
		{
			this.circuit = circuit;
		}
	}
}
