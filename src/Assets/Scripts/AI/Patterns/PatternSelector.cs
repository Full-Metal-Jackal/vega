using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
	public class PatternSelector : MonoBehaviour
	{
		[SerializeField]
		protected DefaultPattern defaultPattern;
		[SerializeField]
		protected AgressivePattern agressivePattern;
		[SerializeField]
		protected DeffensivePattern deffensivePattern;

		private const float agressiveTreshhold = 0.75f;
		private const float deffensiveTreshhold = 0.25f;

		protected struct EnvironmentData
		{
			public float mobHp;
			public float targetHp;
			public int targetKS;
			public float distanceFromTarget;
		}

		public virtual CombatPattern SelectPattern(AIManager aiManager)
		{

			EnvironmentData data = CollectData(aiManager);
			CombatPattern pattern = defaultPattern;

			/*
			 * MinMaxScaller сюда воткнуть
			 */
			float statusTmp = data.mobHp / data.targetHp - data.targetKS;

			print("Status value: " + statusTmp);

			//Вычисляется status моба по формуле
			float status = 0.8f;

			if (status <= deffensiveTreshhold)
			{
				pattern = deffensivePattern;
			}
			else if (status >= agressiveTreshhold)
			{
				pattern = agressivePattern;
			}
			else
			{
				pattern = defaultPattern;
			}
			print($"{aiManager.Possessed} switched to pattern {pattern}");
			return pattern;
		}

		protected EnvironmentData CollectData(AIManager aiManager)
		{
			//aiManager.distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			float distanceFromTarget = Vector3.Distance(aiManager.currentTarget.transform.position, aiManager.transform.position);
			float mobHp = aiManager.Possessed.Health;
			float targetHp = aiManager.currentTarget.Health;
			int targetKS = 1;

			EnvironmentData data = new EnvironmentData();
			data.distanceFromTarget = distanceFromTarget;
			data.mobHp = mobHp;
			data.targetHp = targetHp;
			data.targetKS = targetKS;

			return data;

		}
	}
}

