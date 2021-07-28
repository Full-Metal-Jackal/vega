using UnityEngine;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class DataPinWidget : PinWidget<DataPin>
	{
		[field: SerializeField]
		public UnityEngine.UI.Text Value { get; private set; }

		public override bool TryConnect(Pin other)
		{
			bool connected = false;

			if (Pin is DataInput input && other is DataOutput output)
				connected = output.Connect(input);
			else if (other is DataInput && Pin is DataOutput)
				connected = (Pin as DataOutput).Connect(other as DataInput);

			return connected;
		}

		public void UpdateValue(Data value)
		{
			Value.text = $"{value}";
		}

		public override void Setup(DataPin pin)
		{
			base.Setup(pin);

			pin.OnValueChanged += UpdateValue;
			UpdateValue(pin.Value);
		}
	}
}
