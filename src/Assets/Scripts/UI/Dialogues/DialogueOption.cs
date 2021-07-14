using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Dialogue
{
	[RequireComponent(typeof(Button))]
	public class DialogueOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public bool Initialized { get; private set; } = false;


		[SerializeField]
		private Image selector;

		private Button button;
		private TextMeshProUGUI textMesh;

		private void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return;
			}

			textMesh = GetComponent<TextMeshProUGUI>();
			button = GetComponent<Button>();

			ToggleSelect(false);
			Initialized = true;
		}

		public void ToggleSelect(bool toggle)
		{
			// <TODO> Add select sound here.
			selector.enabled = toggle;
		}

		public void OnPointerEnter(PointerEventData eventData) =>
			ToggleSelect(true);

		public void OnPointerExit(PointerEventData eventData) =>
			ToggleSelect(false);
	}
}
