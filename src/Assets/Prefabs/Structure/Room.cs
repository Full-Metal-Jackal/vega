using UnityEngine;

public class Room : MonoBehaviour
{
	private Transform walls;
	private Transform furniture;
	private Transform shade;

	[SerializeField]
	private Material[][] defaultMat;
	[SerializeField]
	private Material transparentMat;

	void Start()
    {
		walls = transform.Find("Walls");
		furniture = transform.Find("Furniture");
		shade = transform.Find("ShadeRoomPlane");
		Transform[] prefabs = walls.GetComponentsInChildren<Transform>();
		if (defaultMat == null)
		{
			defaultMat = new Material[prefabs.Length][];
			for (int i = 0; i < prefabs.Length; i++) //May need to update it in the future
			{
				Renderer[] localRenderer = prefabs[i].GetComponentsInChildren<Renderer>();
				defaultMat[i] = new Material[2];
				defaultMat[i][0] = localRenderer[0].material; 
				defaultMat[i][1] = localRenderer[1].material;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		print(defaultMat[0]);
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
			Transform[] prefabs = walls.GetComponentsInChildren<Transform>();
			for (int i = 0; i < prefabs.Length; i++)
			{
				Renderer[] localRenderer = prefabs[i].GetComponentsInChildren<Renderer>();
				localRenderer[0].material = transparentMat;
				localRenderer[1].material = transparentMat;
			}
		}
	}

	void ShowWalls()
	{
		if (walls != null)
		{
			Transform[] prefabs = walls.GetComponentsInChildren<Transform>();
			for (int i = 0; i < prefabs.Length; i++)
			{
				Renderer[] localRenderer = prefabs[i].GetComponentsInChildren<Renderer>();
				localRenderer[0].material = defaultMat[i][0];
				localRenderer[1].material = defaultMat[i][1];
			}
		}
	}
}
