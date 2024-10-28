using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTransform : MonoBehaviour
{
    public RouteRecords records;
    public PlayerController player; 
    public int recordIdx = 0;

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 0.9f)
        {
            if(recordIdx < 19)
                recordIdx++;

            else
                recordIdx = 0;

            gameObject.transform.position = records.positions[recordIdx];
        }
    }
}
