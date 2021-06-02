using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	public bool Selectable { get; set; }

	/// <summary>
	/// Called when the mob uses the object.
	/// </summary>
	/// <param name="mob">The user of the object.</param>
	/// <returns>true if the object was used successfully, false otherwise.</returns>
	public bool OnUsed(Mob mob);
	public bool CanBeUsedBy(Mob mob);
}
