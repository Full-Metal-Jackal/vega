using Circuitry;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class DataPinWidget : PinWidget
	{
		public override bool TryConnect(Pin other)
		{
			bool connected = false;

			if (pin is DataInput input && other is DataOutput output)
				connected = output.Connect(input);
			else if (other is DataInput && pin is DataOutput)
				connected = (pin as DataOutput).Connect(other as DataInput);

			return connected;
		}
	}
}
