namespace Circuitry
{
	public class InputButton : Circuit
	{
		public override void Setup()
		{
			AddPulseOutput("On pressed");
		}
	}
}
