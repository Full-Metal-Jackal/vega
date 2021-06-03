/*
 * Detecting walls and changing their material to transparent 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsDetectionManager : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private GameObject walls; // <TODO> It is necessary that when loading the lvl,
                              // the tags of all child objects of the "Walls" object are set to " Detectable"
    private int layerMask = 1 << 3; // <TODO> Understand why masks don't work
    [SerializeField] private string detectableTag = "Detectable";
    [SerializeField] private Material highlighMat;
    [SerializeField] private Material defaultMat;

    private Transform curentDetection;
    void Start()
    {
        player = GameObject.Find("Human");
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, (player.transform.position - cam.transform.position), out hit))
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
                var detectionGr = detection.parent;
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