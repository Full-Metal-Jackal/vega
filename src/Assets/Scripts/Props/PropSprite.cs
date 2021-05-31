using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSprite : MonoBehaviour
{
    void LateUpdate()
    {
        if(Camera.current)
		{
            transform.LookAt(Camera.current.transform);
            transform.Rotate(Vector3.up, 180f);
        }
    }
}
