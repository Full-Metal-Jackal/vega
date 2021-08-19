using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Loading
{
	public class LoadingProgressBar : MonoBehaviour
	{
		[SerializeField]
		private Image bar;

		[field: SerializeField]
		public Text Text { get; private set; }

		private float __progress;
		public float Progress
		{
			get => __progress;
			set
			{
				__progress = Mathf.Clamp01(value);

				Vector3 scale = bar.transform.localScale;
				scale.x = __progress;
				bar.transform.localScale = scale;
			}
		}
	}
}
