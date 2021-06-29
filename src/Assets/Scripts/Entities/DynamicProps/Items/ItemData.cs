using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[field: SerializeField]
	private GameObject Model { get; set; }
	public GameObject PasteModel(Transform transform) => Paste(Model, transform);

	[field: SerializeField]
	private GameObject Collisions { get; set; }
	public GameObject PasteCollisions(Transform transform) => Paste(Collisions, transform);

	[field: SerializeField]
	public GameObject Icon { get; private set; }
	public GameObject PasteIcon(Transform transform) => Paste(Icon, transform);

	[field: SerializeField]
	private GameObject Item { get; set; }
	public GameObject PasteItem(Transform transform) => Paste(Item, transform);

	private GameObject Paste(GameObject gameObject, Transform transform)
	{
		gameObject = Instantiate(gameObject);
		gameObject.transform.SetParent(transform, false);
		return gameObject;
	}
}
