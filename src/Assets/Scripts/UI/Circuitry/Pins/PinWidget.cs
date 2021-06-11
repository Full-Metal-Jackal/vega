using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class PinWidget : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
	{
		public Text label;
		public Circuitry.Pin pin;
		public GameObject trackLinePrefab;
		public bool Initialized { get; private set; } = false;

		private Transform tracksHolder;
		protected RectTransform pinButtonWidget;
		protected RectTransform rectTransform;

		protected HashSet<TrackLineBuilder> lines;
		protected TrackLineBuilder activeLine;

		private void Awake()
		{
			Initialize();
		}

		private bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			rectTransform = GetComponent<RectTransform>();
			tracksHolder = transform.parent.Find("Tracks");

			return Initialized = true;
		}

		public void SetLabel(string text)
		{
			label.text = text;
		}

		public virtual void OnClick()
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			activeLine = CreateLine(rectTransform.position, eventData.position);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			activeLine.UpdateLineEnd(eventData.position);
			activeLine.Destroy();
		}

		public void OnDrag(PointerEventData eventData)
		{
			activeLine.UpdateLineEnd(eventData.position);
		}

		public void OnDrop(PointerEventData eventData)
		{
			if (!eventData.pointerDrag.TryGetComponent(out PinWidget pinWidget))
				return;

			if (TryConnect(pinWidget.pin))
			{
				TrackLineBuilder conneciton = CreateLine(transform.position, pinWidget.transform.position);
				lines.Add(conneciton);
			}
		}

		public virtual bool TryConnect(Circuitry.Pin other) => false;

		public TrackLineBuilder CreateLine(Vector2 from, Vector2 to)
		{
			GameObject trackLineObject = Instantiate(trackLinePrefab);
			trackLineObject.transform.SetParent(tracksHolder);
			
			TrackLineBuilder trackLine = trackLineObject.GetComponent<TrackLineBuilder>();
			trackLine.CreateLine(from, to);

			return trackLine;
		}

	}
}
