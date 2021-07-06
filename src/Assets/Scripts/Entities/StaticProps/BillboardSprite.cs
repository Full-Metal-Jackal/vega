using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    void LateUpdate()
    {
        if(Camera.current)
            transform.forward = Camera.current.transform.forward;
    }
}
