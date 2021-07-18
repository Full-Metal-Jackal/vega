using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
	/// <summary>
	/// This scripts serves as a shortcut for item prefabs, allowing to quickly instantiate its model, collisions, UI Icon or the item itself.
	/// 
	/// Should be placed inside the ItemData prefab.
	/// Must not be used outside of the aforementioned prefab in any way.
	/// 
	/// I fucking hate unity.
	/// </summary>
	public class ItemData : MonoBehaviour
	{
		[SerializeField]
		private GameObject pickablePrefab;

		public Pickable<ItemType> PastePickable<ItemType>(Vector3 position, Quaternion rotation) where ItemType : Item
		{
			Pickable<ItemType> pickable = Paste(
				pickablePrefab,
				Containers.Instance.Items,
				null
			).GetComponent<Pickable<ItemType>>();

			pickable.transform.position = position;
			pickable.transform.rotation = rotation;

			return pickable;
		}

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

		[SerializeField]
		private GameObject sfx;
		public ItemSfxData PasteSfx(Transform transform, int? siblingIdx = null) =>
			Paste(sfx, transform, siblingIdx).GetComponent<ItemSfxData>();

		/// <summary>
		/// Determines what cursor is used with this item.
		/// Leave None to leave the cursor unchanged.
		/// </summary>
		[field: SerializeField]
		public Texture2D Cursor { get; private set; }

		private GameObject Paste(GameObject gameObject, Transform transform, int? siblingIdx)
		{
			gameObject = Instantiate(gameObject);
			gameObject.transform.SetParent(transform, false);
			if (siblingIdx != null)
				gameObject.transform.SetSiblingIndex((int)siblingIdx);

			return gameObject;
		}
	}
}
