using UnityEngine;
using UnityEngine.UI;

using Inventory;

/// <summary>
/// This scripts serves as a shortcut for item prefabs, allowing to quickly instantiate its model, collisions, UI Icon or the item itself.
/// 
/// Should be placed inside the ItemData prefab.
/// Must not be used outside of the aforementioned prefab.
/// 
/// I fucking hate unity.
/// </summary>
public class ItemData : MonoBehaviour
{
	[SerializeField]
	private GameObject model;
	public ItemModelData PasteModel(Transform transform, int? siblingIdx = null) =>
		Paste(model, transform, siblingIdx).GetComponent<ItemModelData>();

	[SerializeField]
	private GameObject collisions;
	public Transform PasteCollisions(Transform transform, int? siblingIdx = null) =>
		Paste(collisions, transform, siblingIdx).transform;

	[SerializeField]
	private GameObject icon;
	public Image PasteIcon(Transform transform, int? siblingIdx = null) =>
		Paste(icon, transform, siblingIdx).GetComponent<Image>();

	[SerializeField]
	private GameObject item;
	public Item PasteItem(Transform transform, int? siblingIdx = null) =>
		Paste(item, transform, siblingIdx).GetComponent<Item>();

	private GameObject Paste(GameObject gameObject, Transform transform, int? siblingIdx)
	{
		gameObject = Instantiate(gameObject);
		gameObject.transform.SetParent(transform, false);
		if (siblingIdx != null)
			gameObject.transform.SetSiblingIndex((int)siblingIdx);

		return gameObject;
	}
}
