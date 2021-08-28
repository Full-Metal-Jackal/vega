using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class HoverableFontSwitcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[SerializeField]
		private TMPro.TextMeshProUGUI text;

		[SerializeField]
		private TMPro.TMP_FontAsset normalFont;
		[SerializeField]
		private TMPro.TMP_FontAsset highlightedFont;

		private void Start() => SetHighlight(false);

		public void OnPointerEnter(PointerEventData eventData) => SetHighlight(true);
		public void OnPointerExit(PointerEventData eventData) => SetHighlight(false);

		public void SetHighlight(bool enabled) =>
			text.font = enabled ? highlightedFont : normalFont;
	}
}
