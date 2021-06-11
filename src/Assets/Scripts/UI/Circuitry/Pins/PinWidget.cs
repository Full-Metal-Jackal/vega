using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI
{
	public abstract class PinWidget : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
	{
		public Text label;
		public Circuitry.Pin pin;

		public HashSet<UIGridRenderer> lines;
		public UIGridRenderer line;

		public void SetLabel(string text)
		{
			label.text = text;
		}

		public virtual void OnClick()
		{
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.Log("OnPointerDown");
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			Debug.Log($"Started drag at {eventData.position}");
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log($"Ended drag at {eventData.position}");
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		public void OnDrop(PointerEventData eventData)
		{
			Debug.Log("OnDrop");
		}
	}
}
