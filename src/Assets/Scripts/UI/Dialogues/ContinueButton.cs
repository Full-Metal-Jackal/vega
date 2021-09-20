using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;

namespace UI.Dialogue
{
	public class ContinueButton : DialogueButton
	{
		public void Toggle(bool toggle) =>
			gameObject.SetActive(toggle);

		public void UpdateNode(SpeechNode node) =>
			Node = node;
	}
}
