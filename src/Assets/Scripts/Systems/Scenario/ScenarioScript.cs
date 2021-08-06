using UnityEngine;

using TriggerSystem;

namespace Scenario
{
	public abstract class ScenarioScript : MonoBehaviour
	{
		public TriggerAction OnFinish;

		protected virtual void Finish()
		{
			OnFinish?.Invoke();
		}
	}
}
