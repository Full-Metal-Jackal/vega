using UnityEngine;

public class Room : MonoBehaviour
{
	private Transform walls;
	private Transform furniture;
	private Transform shade;

	[SerializeField]
	private Material defaultMat;
	[SerializeField]
	private Material transparentMat;

	void Start()
    {
		walls = transform.Find("Walls");
		furniture = transform.Find("Furniture");
		shade = transform.Find("ShadeRoomPlane");
		if (defaultMat == null)
		{
			defaultMat = walls.GetComponentInChildren<Renderer>().material; //May need to update it in the future
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		ShowFurniture();
		HideShadow();
		HideWalls();
	}

	private void OnTriggerExit(Collider other)
	{
		HideFurniture();
		ShowShadow();
		ShowWalls();
	}

	void ShowFurniture()
	{
		if (furniture != null)
		{
			Renderer[] selectionRenderer = furniture.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].enabled = true;
			}
		}
	}

	void HideFurniture()
	{
		if (furniture != null)
		{
			Renderer[] selectionRenderer = furniture.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].enabled = false;
			}
		}
	}

	void HideShadow()
	{
		if (shade != null)
		{
			Renderer[] selectionRenderer = shade.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].enabled = false;
			}
			shade.GetComponent<RoomShade>().changeState();
		}
	}

	void ShowShadow()
	{
		if (shade != null)
		{
			Renderer[] selectionRenderer = shade.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].enabled = true;
			}
		}
	}

	void HideWalls()
	{
		if (walls != null)
		{
			Renderer[] selectionRenderer = walls.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].material = transparentMat;
			}
		}
	}

	void ShowWalls()
	{
		if (walls != null)
		{
			Renderer[] selectionRenderer = walls.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].material = defaultMat;
			}
		}
	}
}
