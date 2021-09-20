using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class MiddleButtonScrollRect : ScrollRect
	{
		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Middle)
			{
				eventData.button = PointerEventData.InputButton.Left;
				base.OnBeginDrag(eventData);
			}
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Middle)
			{
				eventData.button = PointerEventData.InputButton.Left;
				base.OnEndDrag(eventData);
			}
		}

		public override void OnDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Middle)
			{
				eventData.button = PointerEventData.InputButton.Left;
				base.OnDrag(eventData);
			}
		}
	}
}
