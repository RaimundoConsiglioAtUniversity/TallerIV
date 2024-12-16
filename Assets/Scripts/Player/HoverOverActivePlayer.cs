using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOverActivePlayer : MonoBehaviour
{
    public Vector3 offset = Vector3.up;

    void Update()
    {
        transform.position = PlayerInput.Instance.pony.transform.position + offset;
    }
}
