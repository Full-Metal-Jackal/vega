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
		[field: SerializeField]
		private RectTransform buttonRect;

		[SerializeField]
		private TMPro.TextMeshProUGUI ButtonText;

		[SerializeField]
		private bool inversed;

		[SerializeField]
		private float additionalOffset = 0f;

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

		private float transitionTarget = 0f;
		private float transitionProgress = 0f;

		public void Toggle() => Toggle(!IsUnfold);

		public virtual void Toggle(bool unfold)
		{
			enabled = true;
			IsUnfold = unfold;
			transitionTarget = unfold ? 1f : 0f;
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
			Vector3 pos = wingRect.anchoredPosition;

			if (orientation is Orientation.Horizontal)
				pos.x = inversed
					? Mathf.Lerp(buttonRect.rect.width - additionalOffset, wingRect.rect.width, transitionProgress)
					: Mathf.Lerp(buttonRect.rect.width - additionalOffset - wingRect.rect.width, 0, transitionProgress);
			else
				pos.y = inversed
					? Mathf.Lerp(additionalOffset + wingRect.rect.height - buttonRect.rect.height, 0, transitionProgress)
					: Mathf.Lerp(buttonRect.rect.height - additionalOffset, wingRect.rect.height, transitionProgress);

			wingRect.anchoredPosition = pos;
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
