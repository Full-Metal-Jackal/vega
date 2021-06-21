using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CircuitConstructor
{
	public interface ITriggerable<T>
	{
		public bool Trigger(T caller, string eventLabel = "");
	}
}
