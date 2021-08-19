using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple commentary, same for every person.
/// </summary>
public class Commentable : Interaction
{
	[SerializeField]
	private string defaultCommentary;
	[SerializeField]
	private float defaultAdditiveDelay = 0;

	[SerializeField]
	private List<Commentary> commentaries;

	private Commentary FindCharactersCommentary(Mob mob)
	{
		foreach (Commentary commentary in commentaries)
			if (mob == commentary.Character)
				return commentary;
		return null;
	}

	public override bool OnUse(Mob mob)
	{
		string text = defaultCommentary;
		float additiveDelay = defaultAdditiveDelay;

		if (FindCharactersCommentary(mob) is Commentary commentary)
		{
			text = commentary.Text;
			additiveDelay = commentary.AdditiveDelay;
		}

		mob.Speaker.Speak(text, additiveDelay, true);
		mob.Speaker.DisappearAtDistance(Entity.transform);

		return true;
	}
}
