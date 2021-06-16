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
	private int detectionRange = 1000;
	public int layerMask;
	[SerializeField] private string detectableTag = "Detectable";
	[SerializeField] private Material highlighMat;
	[SerializeField] private Material defaultMat;

	private Transform curentDetection;
	void Start()
	{
		wallsId = LayerMask.NameToLayer("StopCamRaycast");
		Debug.Log(wallsId);
		layerMask = 1 << wallsId;
		player = GameObject.Find("Player").GetComponent<PlayerController>().possessAtStart;  //Still not optimal I think.
		Debug.Log(player);
		cam = Camera.main;
	}

	void Initialize()
	{
		
	}

	
	void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, player.transform.position - cam.transform.position, out hit, detectionRange, layerMask))
		{
			if (curentDetection != null)
			{
				Renderer[] selectionRenderer = curentDetection.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].material = defaultMat;
					curentDetection = null;
				}
			}
			var detection = hit.transform;
			if (detection.CompareTag(detectableTag))
			{
				Debug.DrawRay(cam.transform.position, player.transform.position - cam.transform.position, Color.red);
				var detectionGr = detection.parent;;
				Renderer[] selectionRenderer = detectionGr.GetComponentsInChildren<Renderer>();
				if (selectionRenderer != null)
				{
					for (int i = 0; i < selectionRenderer.Length; i++)
					{
						selectionRenderer[i].material = highlighMat;
					}
					curentDetection = detectionGr;
				}
			}
		}
		else
		{
			Debug.DrawRay(cam.transform.position, player.transform.position - cam.transform.position, Color.green);
			if (curentDetection != null)
			{
				Renderer[] selectionRenderer = curentDetection.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < selectionRenderer.Length; i++)
				{
					selectionRenderer[i].material = defaultMat;
					curentDetection = null;
				}
			}
		}
	}
}
