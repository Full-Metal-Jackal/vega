using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UI.CircuitConstructor
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class PinWidget : MonoBehaviour
	{
		public bool Initialized { get; protected set; } = false;

		public RectTransform RectTransform { get; private set; }

		[field: SerializeField]
		public Text Label { get; private set; }

		[field: SerializeField]
		public RectTransform ButtonRectTransform { get; private set; }

		public readonly HashSet<TrackLineBuilder> tracks = new HashSet<TrackLineBuilder>();

		[field: SerializeField]
		protected TrackLineBuilder TrackLinePrefab { get; private set; }

		[field: SerializeField]
		protected RectTransform TracksHolder { get; private set; }

		protected TrackLineBuilder activeTrack;

		private void Awake() => Initialize();

		protected virtual bool Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return false;
			}

			RectTransform = GetComponent<RectTransform>();

			return Initialized = true;
		}

		public void SetLabel(string text) => Label.text = text;

		public void UpdateLines()
		{
			foreach (TrackLineBuilder track in tracks)
				track.UpdateLine();
		}

		public virtual void Setup(Circuitry.Pin pin)
		{
		}
	}

	public abstract class PinWidget<PinType> : PinWidget, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler where PinType : Circuitry.Pin
	{
		public PinType Pin { get; protected set; }

		public override void Setup(Circuitry.Pin pin) => Setup(pin as PinType);  // Блять.
		public virtual void Setup(PinType pin)
		{
			Pin = pin;
			SetLabel(pin.label);
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
			if (!eventData.pointerDrag.TryGetComponent(out PinWidget<PinType> pinWidget))
				return;

			if (TryConnect(pinWidget.Pin))
			{
				TrackLineBuilder conneciton = CreateTrackBuilder();
				conneciton.CreateLine(this, pinWidget);
				tracks.Add(conneciton);
				pinWidget.tracks.Add(conneciton);
			}
		}

		public virtual bool TryConnect(Circuitry.Pin other) => false;

		public TrackLineBuilder CreateLine(Vector2 from, Vector2 to)
		{
			TrackLineBuilder trackLine = CreateTrackBuilder();
			trackLine.CreateLine(from, to);
			return trackLine;
		}

		private TrackLineBuilder CreateTrackBuilder()
		{
			TrackLineBuilder trackLine = Instantiate(TrackLinePrefab);
			trackLine.transform.SetParent(TracksHolder, false);

			return trackLine;
		}
	}
}
