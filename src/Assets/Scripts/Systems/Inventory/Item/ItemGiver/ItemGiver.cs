using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Inventory;

public class ItemGiver : MonoBehaviour
{
	[SerializeField]
	private ItemData itemToGive;

	[SerializeField]
	private Mob mob;

	public virtual void GiveItem()
	{
		if (mob == null)
			throw new System.Exception($"No {mob} assigned to item giver");

		if (itemToGive == null)
			throw new System.Exception($"No item data assigned for {mob}");

		if (!(itemToGive.PasteItem(Containers.Instance.Items) is Item item))
			throw new System.Exception($"Invalid item data assigned for {mob}");

		mob.PickUpItem(item);

		Destroy(this);
	}
}
