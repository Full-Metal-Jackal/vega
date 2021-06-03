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
    private GameObject walls; // <TODO> Нужно чтоб при загрузке лвл тэги всех дочерних объектов объекта "Walls" устанавливались на "Detectable"
    private int layerMask = 1 << 3; // <TODO> ПОнять почему маски не работают
    // Start is called before the first frame update
    [SerializeField] private string detectableTag = "Detectable";
    [SerializeField] private Material highlighMat;
    [SerializeField] private Material defaultMat;

    private Transform _detection;
    void Start()
    {
        player = GameObject.Find("Human");
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = cam.ScreenPointToRay(player.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, (player.transform.position - cam.transform.position), out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(detectableTag))
            {
                var selectionGr = selection.parent;
                Debug.Log(selectionGr);
                Renderer[] selectionRenderer = selectionGr.GetComponentsInChildren<Renderer>();
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
            Debug.Log("Nothing hitted");
        }
        
    }
}
