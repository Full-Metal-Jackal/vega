using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class HumanPatternSelector : PatternSelector
	{
		private const float agressiveTreshhold = 0.5f;
		private const float deffensiveTreshhold = 0.05f;

		public override CombatPattern SelectPattern(AIManager aiManager)
		{

			EnvironmentData data = CollectData(aiManager);
			float status = data.targetHp / 100;

			CombatPattern pattern;
			if (status <= deffensiveTreshhold)
			{
				pattern = deffensivePattern;
			}
			else if (status <= agressiveTreshhold)
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
