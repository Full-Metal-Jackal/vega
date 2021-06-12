using UnityEngine;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class DataPinWidget : PinWidget
	{
		public UnityEngine.UI.Text value;

		public override bool TryConnect(Pin other)
		{
			bool connected = false;

			if (pin is DataInput input && other is DataOutput output)
				connected = output.Connect(input);
			else if (other is DataInput && pin is DataOutput)
				connected = (pin as DataOutput).Connect(other as DataInput);

			return connected;
		}

		public override bool Trigger(Pin caller)
		{
			if (!base.Trigger(caller))
				return false;

			UpdateValue();

			return true;
		}

		public void UpdateValue()
		{
			value.text = $"{(pin as DataPin)?.Value}";
		}

		public override void Setup(Pin pin)
		{
			base.Setup(pin);
			UpdateValue();
		}
	}
}
