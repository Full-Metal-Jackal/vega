using UnityEngine;

/// <summary>
/// Simple commentary, same for every person.
/// </summary>
public class Commentary : Interaction
{
	[SerializeField]
	private string commentary;

	[SerializeField]
	private float additiveDelay;

	public override bool OnUse(Mob mob)
	{
		mob.Speaker.Speak(commentary, additiveDelay, true);
		return true;
	}
}
