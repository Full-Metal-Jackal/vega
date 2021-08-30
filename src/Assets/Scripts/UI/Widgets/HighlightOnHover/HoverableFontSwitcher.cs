using UnityEngine;

namespace UI
{
	public class HoverableFontSwitcher : HoverableHighlight
	{
		[SerializeField]
		private TMPro.TextMeshProUGUI text;

		[SerializeField]
		private TMPro.TMP_FontAsset normalFont;
		[SerializeField]
		private TMPro.TMP_FontAsset highlightedFont;

		public override void SetHighlight(bool enabled) =>
			text.font = enabled ? highlightedFont : normalFont;
	}
}
