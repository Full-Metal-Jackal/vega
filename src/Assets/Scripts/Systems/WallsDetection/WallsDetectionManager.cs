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
	private readonly int detectionRange = 1000;
	public int layerMask;
	[SerializeField] 
	private Material highlighMat;
	[SerializeField] 
	private Material defaultMat;

	private Transform curentWallDetection;
	void Start()
	{
		wallsId = LayerMask.NameToLayer("StopCamRaycast");
		layerMask = 1 << wallsId;
		player = GameObject.Find("Player").GetComponentInChildren<PlayerController>().possessAtStart;
		cam = Camera.main;
	}

	void RayToWalls()
	{
		Renderer[] selectionRenderer;
		if (Physics.Raycast(cam.transform.position, player.transform.position - cam.transform.position, out RaycastHit hit, detectionRange, layerMask))
		{
			if (curentWallDetection != null)
			{
				selectionRenderer = curentWallDetection.GetComponents<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].enabled = true;
					curentWallDetection = null;
				}
			}
			var detection = hit.transform;
			Debug.DrawRay(cam.transform.position, player.transform.position - cam.transform.position, Color.red);
			selectionRenderer = detection.GetComponents<Renderer>();
			if (selectionRenderer != null)
			{
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].enabled = false;
				}
				curentWallDetection = detection;
			}
		}
		else
		{
			Debug.DrawRay(cam.transform.position, player.transform.position - cam.transform.position, Color.green);
			if (curentWallDetection != null)
			{
				selectionRenderer = curentWallDetection.GetComponents<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].enabled = true;
					curentWallDetection = null;
				}
			}
		}
	}

	void Update()
	{
		RayToWalls();
	}
}
