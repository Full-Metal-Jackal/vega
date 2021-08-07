using UnityEngine;

namespace Scenario
{
	/// <summary>
	/// Nodes serve as middlemen in sequences of scenario scripts to make re-routing simple.
	/// </summary>
	public class ScenarioNode : ScenarioScript
	{
		public void Activate() => Finish();
	}
}
