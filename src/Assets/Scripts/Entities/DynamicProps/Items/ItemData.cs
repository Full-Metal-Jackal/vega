using System.Collections;
using System.Collections.Generic;
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
	public ItemModelData PasteModel(Transform transform) =>
		Paste(model, transform).GetComponent<ItemModelData>();

	[SerializeField]
	private GameObject collisions;
	public Transform PasteCollisions(Transform transform) =>
		Paste(collisions, transform).transform;

	[SerializeField]
	private GameObject icon;
	public Image PasteIcon(Transform transform) =>
		Paste(icon, transform).GetComponent<Image>();
	//{
	//	Image image = icon.GetComponentInChildren<Image>();
	//	return Paste(image.gameObject, transform).GetComponent<Image>();
	//}

	[SerializeField]
	private GameObject item;
	public Item PasteItem(Transform transform) =>
		Paste(item, transform).GetComponent<Item>();

	private GameObject Paste(GameObject gameObject, Transform transform)
	{
		gameObject = Instantiate(gameObject);
		gameObject.transform.SetParent(transform, false);
		return gameObject;
	}
}
