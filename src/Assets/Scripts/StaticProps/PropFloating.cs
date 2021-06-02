using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropFloating : MonoBehaviour
{
    public const float floatingRange = .1f;
    public const float floatingSpeed = 2f;
    void Update()
    {
        float shift = floatingRange * Mathf.Cos(Time.time * floatingSpeed);
        transform.position += new Vector3(0, Time.deltaTime * shift, 0);
    }
}
