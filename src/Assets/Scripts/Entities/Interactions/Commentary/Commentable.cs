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
			if (mob.Traits == commentary.Character)
				return commentary;

		Debug.LogWarning($"{mob} has nothing to say about {Entity}!");
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

		mob.Speaker.DisappearAtDistance(mob.Speaker.transform.position);
		mob.Speaker.Speak(text, additiveDelay, true);

		return true;
	}
}
