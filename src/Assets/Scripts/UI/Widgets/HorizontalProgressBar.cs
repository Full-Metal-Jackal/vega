using System;
using UnityEngine;

namespace UI
{
	public class HorizontalProgressBar : MonoBehaviour
	{
		[SerializeField]
		private RectTransform barTransform;

		private float __value;
		public float Value
		{
			get => __value;
			set
			{
				__value = Mathf.Clamp01(value);
				barTransform.localPosition = new Vector3(
					-barTransform.rect.width * (1 - __value),
					0f, 0f
				);
			}
		}
	}
}
