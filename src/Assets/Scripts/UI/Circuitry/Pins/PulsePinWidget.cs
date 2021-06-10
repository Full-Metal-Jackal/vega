using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;

namespace UI
{
	public class PulsePinWidget : PinWidget
	{
		public override void OnClick()
		{
			base.OnClick();

			Pulse();
		}

		public void Pulse() => (pin as PulsePin)?.Pulse();
	}
}
