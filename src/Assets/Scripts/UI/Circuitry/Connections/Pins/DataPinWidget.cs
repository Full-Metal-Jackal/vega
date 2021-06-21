using UnityEngine;
using UnityEngine.EventSystems;

using Circuitry;

namespace UI.CircuitConstructor
{
	public class DataPinWidget : PinWidget
	{
		[field: SerializeField]
		public UnityEngine.UI.Text Value { get; private set; }

		public override bool TryConnect(Pin other)
		{
			bool connected = false;

			if (BoundPin is DataInput input && other is DataOutput output)
				connected = output.Connect(input);
			else if (other is DataInput && BoundPin is DataOutput)
				connected = (BoundPin as DataOutput).Connect(other as DataInput);

			return connected;
		}

		public override bool Trigger(Pin caller, string eventLabel)
		{
			if (!base.Trigger(caller, eventLabel))
				return false;

			switch (eventLabel)
			{
			case "valueChange":
				UpdateValue();
				break;
			default:
				Debug.LogWarning($"{this} encountered unsupported event: {eventLabel}");
				return false;
			}

			return true;
		}

		public void UpdateValue()
		{
			Value.text = $"{(BoundPin as DataPin)?.Value}";
		}

		public override void Setup(Pin pin)
		{
			base.Setup(pin);
			UpdateValue();
		}
	}
}
