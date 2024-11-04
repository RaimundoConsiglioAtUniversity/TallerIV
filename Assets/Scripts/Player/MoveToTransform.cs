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
        if (records.PositionIsValid && Vector3.Distance(records.positions[recordIdx], player.transform.position) < 1.2f)
        {
            if(recordIdx < 19)
                recordIdx++;

            else
                recordIdx = 0;
        }

        gameObject.transform.position = records.positions[recordIdx];
    }
}
