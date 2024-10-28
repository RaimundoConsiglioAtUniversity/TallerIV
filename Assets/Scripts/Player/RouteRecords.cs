using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouteRecords : MonoBehaviour
{
    public bool IsRecording {get; private set; } = false;
    public PlayerController player;
    public Vector3 [] positions = new Vector3[20];
    public int posIdx = 0;

    public void AddToRegister(PlayerController player)
    {
        if (!player || !player.groundCheck.isGrounded || (positions[posIdx] == player.transform.position || Vector3.Distance(positions[posIdx], player.transform.position) < 0.1f))
            return;
        
        positions[posIdx] = player.transform.position;

        if(posIdx < 19)
            posIdx++;

        else
            posIdx = 0;
        
    }

	void Awake()
	{
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = player.transform.position;
        }

        StartRecording();
    }

    public void StartRecording()
    {
        IsRecording = true;
        StartCoroutine(Record());
    }

    public void EndRecording() => IsRecording = false;

    private IEnumerator Record()
    {
        while(IsRecording)
        {
            AddToRegister(player);

            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }
}
