using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void ClampVelocity(this Rigidbody2D rb, float clampX, float clampY)
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -clampX, clampX), Mathf.Clamp(rb.velocity.y, -clampY, clampY));
    }
    public static void AccelerateTo(this Rigidbody2D rb, Vector2 velocity, float accelerationTime, float lerpVal, out float newLerpVal)
    {
        rb.velocity = Vector2.Lerp(rb.velocity, velocity, lerpVal);
        lerpVal += Time.deltaTime / accelerationTime;
        Mathf.Clamp01(lerpVal);
        
        newLerpVal = lerpVal;
    }

    public static void ResetOnChange(this float f, float a, float b, float def = 0f)
    {
        if(a != b)
        {
            f = def;
            a = b;
        }
    }
}
