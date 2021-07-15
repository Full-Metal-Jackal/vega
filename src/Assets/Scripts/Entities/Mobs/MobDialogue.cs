using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UI.Dialogue;

[RequireComponent(typeof(Mob))]
public class MobDialogue : Interaction
{
	public Mob Mob => Entity as Mob;

	[field: SerializeField]
	public NPCConversation Conversation { get; private set; }

	public override bool OnUse(Mob mob)
	{
		if (mob != PlayerController.Instance.Possessed)
			return false;

		Debug.Log($"{mob} started dialogue with {Mob}");
		DialogueWindow.Instance.Open(Conversation);
		return true;
	}
}
