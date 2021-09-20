using Circuitry;
using UnityEngine;

namespace UI.CircuitConstructor
{
	public class PulsePinWidget : PinWidget<PulsePin>
	{
		public override void OnClick()
		{
			base.OnClick();

			Pulse();
		}

		public void Pulse() => Pin.Pulse();

		public override bool TryConnect(Pin other)
		{
			bool connected = false;

			if (Pin is PulseInput input && other is PulseOutput output)
				connected = output.Connect(input);
			if (other is PulseInput && Pin is PulseOutput)
				connected = (Pin as PulseOutput).Connect(other as PulseInput);

			return connected;
		}
	}
}
