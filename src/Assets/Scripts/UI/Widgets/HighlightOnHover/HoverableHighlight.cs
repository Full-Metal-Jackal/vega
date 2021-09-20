using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public abstract class HoverableHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		private void Start() => SetHighlight(false);

		public void OnPointerEnter(PointerEventData eventData) => SetHighlight(true);
		public void OnPointerExit(PointerEventData eventData) => SetHighlight(false);

		public abstract void SetHighlight(bool enabled);
	}
}
