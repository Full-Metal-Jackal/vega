using UnityEngine;
using UnityEngine.UI;

using UI;

public class PlayerStaminaCell : AlphaIndicator
{
	[SerializeField]
	private Image bloomImage;

	public override float Value
	{
		set
		{
			base.Value = value;

			Color color = bloomImage.color;
			color.a = Mathf.Clamp01(value);
			bloomImage.color = color;
		}
	}
}
