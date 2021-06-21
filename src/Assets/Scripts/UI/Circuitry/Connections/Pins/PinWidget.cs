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

		protected TrackLineBuilder activeTrack;
		
		protected readonly HashSet<TrackLineBuilder> tracks = new HashSet<TrackLineBuilder>();

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
			activeTrack = CreateTrackBuilder();
			activeTrack.CreateLine(Vector3.zero, activeTrack.ScreenToPinLocal(this, eventData.position));
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			activeTrack.Destroy();
		}

		public void OnDrag(PointerEventData eventData)
		{
			activeTrack.UpdateLineEnd(activeTrack.ScreenToPinLocal(this, eventData.position));
		}

		public void OnDrop(PointerEventData eventData)
		{
			if (!eventData.pointerDrag.TryGetComponent(out PinWidget pinWidget))
				return;

			if (TryConnect(pinWidget.BoundPin))
			{
				TrackLineBuilder conneciton = CreateTrackBuilder();
				conneciton.CreateLine(this, pinWidget);
				tracks.Add(conneciton);
				pinWidget.tracks.Add(conneciton);
			}
		}

		public virtual bool TryConnect(Circuitry.Pin other) => false;

		private TrackLineBuilder CreateTrackBuilder()
		{
			GameObject trackLineObject = Instantiate(trackLinePrefab);
			trackLineObject.transform.SetParent(tracksHolder, false);

			return trackLineObject.GetComponent<TrackLineBuilder>();
		}

		public TrackLineBuilder CreateLine(Vector2 from, Vector2 to)
		{
			TrackLineBuilder trackLine = CreateTrackBuilder();
			trackLine.CreateLine(from, to);
			return trackLine;
		}

		public void UpdateLines()
		{
			foreach (TrackLineBuilder track in tracks)
				track.UpdateLine();
		}

		public virtual bool Trigger(Circuitry.Pin caller, string eventLabel) => true;
	}
}
