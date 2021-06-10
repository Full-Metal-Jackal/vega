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

		public HashSet<LineRenderer> lines;
		public LineRenderer line;

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
			line = gameObject.AddComponent<LineRenderer>();
			line.startColor = Color.green;
			line.endColor = Color.green;
			line.positionCount = 2;

			line.SetPosition(0, eventData.position);
			Debug.Log($"Started drag at {eventData.position}");
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log($"Ended drag at {eventData.position}");
		}

		public void OnDrag(PointerEventData eventData)
		{
			line.SetPosition(line.positionCount - 1, eventData.position);
		}

		public void OnDrop(PointerEventData eventData)
		{
			Debug.Log("OnDrop");
		}
	}
}
