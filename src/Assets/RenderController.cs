using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderController : MonoBehaviour
{
    //private Camera cam;

    void Start()
    {
        //cam = GetComponent<Camera>();
        // <TODO> This shit ain't sorting stuff in the right order somehow
        //cam.transparencySortMode = TransparencySortMode.Orthographic;
    }
}
