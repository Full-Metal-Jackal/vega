/*
 * Детекция и изменение видимости стен перекрывающих игрока
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsDetectionManager : MonoBehaviour
{
    private GameObject player;
    private Camera cam;
    private int layerMask = 3;
    // Start is called before the first frame update
    [SerializeField] private Material highlighMat;
    void Start()
    {
        player = GameObject.Find("Human");
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(player.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, (player.transform.position - cam.transform.position), out hit))
        {
            var selection = hit.transform;
            Debug.Log(selection);
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = highlighMat;
            }
        }
        else
        {
            Debug.Log("Nothing hitted");
        }

    }
}
