using UnityEngine;

namespace Inventory
{
	/// <summary>
	/// Represents mob's connection that can hold an item's model.
	/// </summary>
	public class ItemSocket : MonoBehaviour
	{
		public bool IsFree => !item;

		private Item item;
		public Item Item
		{
			get => item;
			set
			{
				item = value;
				item.transform.SetParent(transform, false);
				item.transform.localPosition = Vector3.zero;
				item.transform.localRotation = Quaternion.identity;
			}
		}

		/// <summary>
		/// Destroys everything attached to the socket.
		/// </summary>
		public void Clear()
		{
			foreach (Transform child in transform)
				Destroy(child.gameObject);
		}
	}
}
