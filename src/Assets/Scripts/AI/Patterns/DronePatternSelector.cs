using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class DronePatternSelector : PatternSelector
	{
		private const float agressiveTreshhold = 0.4f;

		public override CombatPattern SelectPattern(AIManager aiManager)
		{
			EnvironmentData data = CollectData(aiManager);
			float status = data.targetHp;

			CombatPattern pattern;
			if (status <= agressiveTreshhold)
			{
				pattern = agressivePattern;
			}
			else
			{
				pattern = defaultPattern;
			}
			return pattern;
		}
	}
}
