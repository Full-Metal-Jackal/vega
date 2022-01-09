using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoPatternSelector : PatternSelector
	{
		private const float deffensiveTreshhold = 2f;
		public override CombatPattern SelectPattern(AIManager aiManager)
		{
			EnvironmentData data = CollectData(aiManager);
			float status = data.distanceFromTarget;

			CombatPattern pattern;
			if (status >= deffensiveTreshhold)
			{
				float n = Random.Range(0, 2);
				// pattern = n <= 0.5f ? defaultPattern : agressivePattern;
				if (n < 1)
					pattern = defaultPattern;
				else
					pattern = agressivePattern;
				pattern = defaultPattern;
			}
			else
			{
				print("Deffense");
				pattern = deffensivePattern;
			}

			return pattern;
		}
	}
}
