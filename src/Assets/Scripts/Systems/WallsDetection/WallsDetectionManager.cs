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
	public int layerMask;
	[SerializeField] private string detectableTag = "Detectable";
	[SerializeField] private Material highlighMat;
	[SerializeField] private Material defaultMat;

	private Transform curentDetection;
	void Start()
	{
		wallsId = LayerMask.NameToLayer("Main Walls");
		layerMask = (1 << wallsId);
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
		if (Physics.Raycast(cam.transform.position, player.transform.position - cam.transform.position, out hit, layerMask))
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
			Debug.Log(detection);
			if (detection.CompareTag(detectableTag))
			{
				var detectionGr = detection.parent;
				Debug.Log(detectionGr);
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
	}
}
