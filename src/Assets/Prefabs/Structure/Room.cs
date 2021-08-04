using UnityEngine;

public class Room : MonoBehaviour
{
	private Transform walls;
	private Transform furniture;
	private Transform shade;
	private Material[] defaultMat;

	[SerializeField]
	private Material transparentMat;

	void Start()
    {
		walls = transform.Find("Walls");
		furniture = transform.Find("Furniture");
		shade = transform.Find("ShadeRoomPlane");
		if (defaultMat == null)
		{
			Renderer[] Renderer = walls.GetComponentsInChildren<Renderer>();
			defaultMat = new Material[Renderer.Length];
			for (int i = 0; i < Renderer.Length; i++)
			{
				defaultMat[i] = Renderer[i].material;
			}
		}

		HideFurniture();
	}

	private void OnTriggerEnter(Collider other)
	{
		//if ((other.transform.parent.TryGetComponent(out Mob mob) && mob.IsPlayer) || mob.canHideWalls))
		if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanHideWalls)
		{
			ShowFurniture();
			HideWalls();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		//if ((other.transform.parent.TryGetComponent(out Mob mob) && mob.IsPlayer) || 
		if (other.transform.parent.TryGetComponent(out Mob mob) && mob.CanHideWalls)
		{
			HideFurniture();
			ShowWalls();
		}  
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
			Renderer[] Renderer = walls.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < Renderer.Length; i++)
			{
				Renderer[i].material = transparentMat;
			}
		}
	}

	void ShowWalls()
	{
		if (walls != null)
		{
			Renderer[] Renderer = walls.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < Renderer.Length; i++)
			{
				Renderer[i].material = defaultMat[i];
			}
		}
	}
}
