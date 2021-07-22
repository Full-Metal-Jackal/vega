using UnityEngine;
using UnityEngine.Events;

namespace TriggerSystem
{
	[System.Serializable]
	public class TriggerAction : UnityEvent
	{
	}

	public abstract class Trigger : MonoBehaviour
	{
		public TriggerAction OnTriggered;

		public virtual void Activate() => OnTriggered?.Invoke();
	}
}
