using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugJumpTime : MonoBehaviour
{
    
    public PlayerController thisPony;
    
    public float debugMaxJumpTime = 0;
    public float debugJumpTimer = 0;

    void Awake()
    {
        thisPony = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (thisPony.charRB.velocity.y > 0.01f)
            debugJumpTimer += Time.deltaTime;

        if (debugJumpTimer > debugMaxJumpTime)
            debugMaxJumpTime = debugJumpTimer;

        if (thisPony.groundCheck.isGrounded)
            debugJumpTimer = 0;
    }
}
