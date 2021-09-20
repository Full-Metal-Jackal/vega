using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
	public abstract class AIState : MonoBehaviour
	{
		public abstract AIState Tick(AIManager aiManager, Mob mob);
	}
}

