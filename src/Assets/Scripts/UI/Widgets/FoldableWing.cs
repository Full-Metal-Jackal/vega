using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class FoldableWing : MonoBehaviour
	{
		[SerializeField]
		private float foldTime = .5f;

		[SerializeField]
		private string foldText = "«";
		[SerializeField]
		private string expandText = "»";

		[field: SerializeField]
		private RectTransform wingRect;

		[SerializeField]
		private TMPro.TextMeshProUGUI ButtonText;

		[SerializeField]
		private bool inversed;

		private bool __isUnfold = false;
		public bool IsUnfold
		{
			get => __isUnfold;
			private set
			{
				__isUnfold = value;

				ButtonText.text = __isUnfold ? foldText : expandText;
			}
		}

		// We have to start the wing offseted because the size fitter makes its width 0 at start.
		private Vector3 initialOffset;

		private float transitionTarget = 0f;
		private float transitionProgress = 0f;

		public void Toggle() => Toggle(!IsUnfold);

		public virtual void Toggle(bool unfold)
		{
			enabled = true;
			IsUnfold = unfold;
			transitionTarget = unfold ? 1f : 0f;
		}

		private void Awake()
		{
			initialOffset = wingRect.localPosition;
		}

		private void Start()
		{
			Toggle(false);
		}

		private void Update()
		{
			if (transitionProgress == transitionTarget)
			{
				enabled = false;
				return;
			}

			float speed = Time.deltaTime / foldTime;
			if (transitionProgress > transitionTarget)
				transitionProgress = Mathf.Max(transitionProgress - speed, 0f);
			else
				transitionProgress = Mathf.Min(transitionProgress + speed, 1f);
		}

		private void OnGUI()
		{
			float reverseMultiplier = inversed ? -1f : 1f;
			Vector3 pos = wingRect.localPosition;

			if (orientation is Orientation.Horizontal)
			{
				float shift = Mathf.Lerp(initialOffset.x, 0, transitionProgress);
				pos.x = shift * reverseMultiplier;
			}
			else
			{
				float shift = Mathf.Lerp(initialOffset.y, 0, transitionProgress);
				pos.y = shift * reverseMultiplier;
			}

			wingRect.localPosition = pos;
		}

		[SerializeField]
		private Orientation orientation;

		public enum Orientation
		{
			Horizontal,
			Vertical
		}
	}
}
