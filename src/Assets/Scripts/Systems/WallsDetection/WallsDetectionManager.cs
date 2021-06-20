/*
 * Detecting walls and changing their material to transparent 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsDetectionManager : MonoBehaviour
{
	private Mob player;
	private Camera cam;
	private int wallsId;
	private int roomsId;
	private int detectionRange = 1000;
	public int layerMask1;
	public int layerMask2;
	[SerializeField] private string detectableTag = "Detectable";
	[SerializeField] private Material highlighMat;
	[SerializeField] private Material defaultMat;

	private Transform curentWallDetection;
	private Transform curentRoomDetection;
	private Transform lastRoomDetection;
	void Start()
	{
		wallsId = LayerMask.NameToLayer("StopCamRaycast");
		roomsId = LayerMask.NameToLayer("RoomDetector");
		layerMask1 = 1 << wallsId;
		layerMask2 = 1 << roomsId;
		player = GameObject.Find("Player").GetComponent<PlayerController>().possessAtStart;  //Still not optimal I think.
		Debug.Log(player);
		cam = Camera.main;
	}

	void Initialize()
	{
		
	}

	void RayToWalls()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, player.transform.position - cam.transform.position, out hit, detectionRange, layerMask1))
		{
			if (curentWallDetection != null)
			{
				Renderer[] selectionRenderer = curentWallDetection.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].material = defaultMat;
					curentWallDetection = null;
				}
			}
			var detection = hit.transform;
			if (detection.CompareTag(detectableTag))
			{
				Debug.DrawRay(cam.transform.position, player.transform.position - cam.transform.position, Color.red);
				var detectionGr = detection.parent;
				Renderer[] selectionRenderer = detectionGr.GetComponentsInChildren<Renderer>();
				if (selectionRenderer != null)
				{
					for (int i = 0; i < selectionRenderer.Length; i++)
					{
						selectionRenderer[i].material = highlighMat;
					}
					curentWallDetection = detectionGr;
				}
			}
		}
		else
		{
			Debug.DrawRay(cam.transform.position, player.transform.position - cam.transform.position, Color.green);
			if (curentWallDetection != null)
			{
				Renderer[] selectionRenderer = curentWallDetection.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].material = defaultMat;
					curentWallDetection = null;
				}
			}
		}
	}

	void RayToZone()
	{
		RaycastHit hit;
		Renderer[] selectionRenderer;
		Transform walls;
		Vector3 direction = new Vector3 (0, 5, 0);
		if (Physics.Raycast(player.transform.position, direction, out hit, detectionRange, layerMask2))
		{
			var detection = hit.transform;
			if (curentRoomDetection == null)
			{
				curentRoomDetection = detection.transform.parent;
				walls = curentRoomDetection.Find("Walls");
				selectionRenderer = walls.GetComponentsInChildren<Renderer>();
				Debug.Log(selectionRenderer.Length);
				if (selectionRenderer != null)
				{
					for (int i = 0; i < selectionRenderer.Length; i++)
					{
						selectionRenderer[i].material = highlighMat;
					}
				}
			}
			
			if (detection.transform.parent != curentRoomDetection)
			{
				walls = curentRoomDetection.Find("Walls");
				selectionRenderer = walls.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].material = defaultMat;
					curentRoomDetection = null;
				}

				Debug.DrawRay(player.transform.position, direction, Color.red);
				curentRoomDetection = detection.transform.parent;
				walls = curentRoomDetection.Find("Walls");
				selectionRenderer = walls.GetComponentsInChildren<Renderer>();
				if (selectionRenderer != null)
				{
					for (int i = 0; i < selectionRenderer.Length; i++)
					{
						selectionRenderer[i].material = highlighMat;
					}
				}
			}
		}
		else
		{
			Debug.DrawRay(player.transform.position, direction, Color.green);
			if (curentRoomDetection != null)
			{
				walls = curentRoomDetection.Find("Walls");
				selectionRenderer = walls.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].material = defaultMat;
					curentRoomDetection = null;
				}
			}
		}
	}
	void Update()
	{
		RayToZone();
		//RayToWalls();
	}
}
