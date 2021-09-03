using UnityEngine;
using DialogueEditor;

[RequireComponent(typeof(Mob))]
public class MobDialogue : Interaction
{
	public Mob Mob => Entity as Mob;

	[field: SerializeField]
	public NPCConversation Conversation { get; private set; }

	public override bool OnUse(Mob mob)
	{
		if (!mob.IsPlayer)
			return false;

		Debug.Log($"{mob} started dialogue with {Mob}");
		Conversation.OpenDialog();

		return true;
	}
}
