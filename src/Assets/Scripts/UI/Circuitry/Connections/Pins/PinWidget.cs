using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class PinWidget : PinWidgetBase, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, ITriggerable<Circuitry.Pin>
	{
		[SerializeField]
		private GameObject trackLinePrefab;
		[SerializeField]
		private RectTransform tracksHolder;
		protected TrackLineBuilder activeLine;
		protected HashSet<TrackLineBuilder> lines = new HashSet<TrackLineBuilder>();

		[SerializeField]
		private RectTransform buttonRectTransform;
		public RectTransform ButtonRectTransform => buttonRectTransform;

		public override void Setup(Circuitry.Pin pin)
		{
			base.Setup(pin);
			EventHandler.Bind(this);
		}

		public virtual void OnClick()
		{
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			activeLine = CreateLine(RectTransform.position, eventData.position);
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

			if (TryConnect(pinWidget.BoundPin))
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

		public virtual bool Trigger(Circuitry.Pin caller) => true;
	}
}
