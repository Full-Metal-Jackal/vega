using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class AlphaIndicator : MonoBehaviour
	{
		[SerializeField]
		private Image image;

		public virtual float Value
		{
			set
			{
				Color color = image.color;
				color.a = Mathf.Clamp01(value);
				image.color = color;
			}
		}
	}
}
