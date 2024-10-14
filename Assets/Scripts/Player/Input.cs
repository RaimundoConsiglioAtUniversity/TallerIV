using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axis
{
    [SerializeField] private float _value;
    public float deadZone;
    public float value
    {
        get => _value;

        set
        {
            if (float.IsFinite(value))
                _value = Mathf.Clamp(value, -1f, 1f);

            else
                _value = 0f;
        }
    }

    public Axis (float value, float deadZone = 0f)
    {
        this.value = value;
        this.deadZone = deadZone;
    }

    public int SteppedValue => Mathf.RoundToInt(Mathf.Abs(value));
    public bool IsPressed => Mathf.Abs(value) > deadZone;
    public int Sign => (int)Mathf.Sign(value);
}
