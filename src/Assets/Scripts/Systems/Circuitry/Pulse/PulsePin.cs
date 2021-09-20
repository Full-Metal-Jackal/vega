namespace Circuitry
{
	public abstract class PulsePin : Pin
	{
		public delegate void PulseEvent();
		public event PulseEvent OnPulse;

		public PulsePin(Circuit circuit, string label) : base(circuit, label)
		{
		}

		public virtual void Pulse()
		{
			Logging.Log($"{circuit}: {this} has been pulsed.");
			OnPulse?.Invoke();
		}
	}
}
