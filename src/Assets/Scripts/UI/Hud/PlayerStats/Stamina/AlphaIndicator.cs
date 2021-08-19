using UnityEngine;
using UnityEngine.UI;

public class AlphaIndicator : MonoBehaviour
{
	[SerializeField]
	private Image image;

	public float Value
	{
		set
		{
			Color color = image.color;
			color.a = Mathf.Clamp01(value);
			image.color = color;
		}
	}
}
