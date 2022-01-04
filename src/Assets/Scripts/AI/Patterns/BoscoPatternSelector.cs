using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class BoscoPatternSelector : PatternSelector
	{
		private const float deffensiveTreshhold = 1f;
		public override CombatPattern SelectPattern(AIManager aiManager)
		{
			EnvironmentData data = CollectData(aiManager);
			float status = data.distanceFromTarget;

			CombatPattern pattern;
			if (status >= deffensiveTreshhold)
			{
				pattern = defaultPattern;
			}
			else
			{
				pattern = deffensivePattern;
			}
			return pattern;
		}
	}
}

