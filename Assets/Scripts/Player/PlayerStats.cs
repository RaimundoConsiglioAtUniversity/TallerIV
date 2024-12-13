using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //--- Walking Vars ---//
    public float walkSpeed = 2.5f;
    public float runMultiplier = 3f;
    public float duckMultiplier = 0.5f;
    public float accelPow = 1.7f;
    public float minAccel = 8f;


    //--- Jumping Vars ---//
    public float jumpStrength = 14;
    public float flapStrength = 8;
    public float upGravity = 24;
    public float downGravity = 42;
    public float coyoteTimeS = 0.05f;
    public float jumpBufferTimeS = 0.05f;
    public int maxFlaps = 3;

    
    public LayerMask obstacleLayers;
    public float maxJumpTime = 0.59f;

    public static PlayerStats Instance => instance;
    private static PlayerStats instance;
    
    void Awake ()
    {
        if (instance != null)
            Destroy(this);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
