using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Dialogue
{
	[RequireComponent(typeof(Button))]
	public abstract class DialogueButton : MonoBehaviour
	{
		public event System.Action<ConversationNode> OnClick;
		public ConversationNode Node { get; protected set; }

		public bool Initialized { get; private set; } = false;

		private void Awake() =>
			Initialize();

		protected virtual void Initialize()
		{
			if (Initialized)
			{
				Debug.LogWarning($"Multiple initialization attempts of {this}!");
				return;
			}

			Initialized = true;
		}

		public virtual void Setup(ConversationNode node)
		{
			Node = node;
		}

		public virtual void OnButtonClicked()
		{
			OnClick?.Invoke(Node);
		}
	}
}
