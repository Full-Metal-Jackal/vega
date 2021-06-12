using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class PulsePinWidget : PinWidget
	{
		public override void OnClick()
		{
			base.OnClick();

			Pulse();
		}

		public void Pulse() => (pin as PulsePin)?.Pulse();

		public override bool TryConnect(Pin other)
		{
			bool connected = false;

			if (pin is PulseInput input && other is PulseOutput output)
				connected = output.Connect(input);
			if (other is PulseInput && pin is PulseOutput)
				connected = (pin as PulseOutput).Connect(other as PulseInput);

			return connected;
		}
	}
}
