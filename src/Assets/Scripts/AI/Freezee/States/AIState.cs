using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FreezeeAI
{
	public abstract class AIState : MonoBehaviour
	{
		public abstract AIState Tick(AIManager aiManager, Mob mob);
	}
}

