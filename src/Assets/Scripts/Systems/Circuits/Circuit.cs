using System.Collections.Generic;
using UnityEngine;

namespace Circuitry
{
	public class Circuit : MonoBehaviour
	{
		/// <summary>
		/// The name of the circuit.
		/// </summary>
		public string label;
		/// <summary>
		/// Description of the circuit.
		/// </summary>
		public string desc;
		public List<Terminal> inPins;
		public List<Terminal> outPins;

		public bool IsSleeping => (cooldown > 0f);
		protected float cooldown = 0f;

		/// <summary>
		/// Makes the circuit inactive for the set amount of time.
		/// An inactive circuit is unable to receive and send pulses.
		/// </summary>
		/// <param name="time">Amount of seconds to sleep.</param>
		public void Sleep(float time)
		{
			cooldown = time;
		}

		private void FixedUpdate()
		{
			cooldown -= Time.fixedDeltaTime;
		}
	}
}
