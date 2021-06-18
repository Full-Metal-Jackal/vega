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
	private int shadesId;
	private int detectionRange = 1000;
	public int layerMask1;
	public int layerMask2;
	[SerializeField] private string detectableTag = "Detectable";
	[SerializeField] private Material highlighMat;
	[SerializeField] private Material defaultMat;

	private Transform curentWallDetection;
	private Transform curentShadeDetection;
	void Start()
	{
		wallsId = LayerMask.NameToLayer("StopCamRaycast");
		shadesId = LayerMask.NameToLayer("RoomShade");
		layerMask1 = 1 << wallsId;
		layerMask2 = 1 << shadesId;
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
		Vector3 direction = new Vector3 (0, 5, 0);
		if (Physics.Raycast(player.transform.position, direction, out hit, detectionRange, layerMask2))
		{
			if (curentShadeDetection != null)
			{
				selectionRenderer = curentShadeDetection.GetComponents<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].enabled = true;
					curentShadeDetection = null;
				}
			}

			Debug.DrawRay(player.transform.position, direction, Color.red);

			var detection = hit.transform;
			selectionRenderer = detection.GetComponents<Renderer>();
			for (int i = 0; i < selectionRenderer.Length; i++)
			{
				selectionRenderer[i].enabled = false;
				curentShadeDetection = detection;
			}

		}
		else
		{
			Debug.DrawRay(player.transform.position, direction, Color.green);
			if (curentShadeDetection != null)
			{
				selectionRenderer = curentShadeDetection.GetComponents<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].enabled = true;
					curentShadeDetection = null;
				}
			}
		}
	}
	void Update()
	{
		RayToZone();
		RayToWalls();
	}
}
