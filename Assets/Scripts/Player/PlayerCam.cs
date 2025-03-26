using UnityEngine;
using Cinemachine;

public class PlayerCam : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;

    private void SetVCamReference()
    {
        if (vCam != null)
            return;
        
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    public void Enable() => Active(true);
    public void Disable() => Active(false);
    public bool Active() => vCam.enabled;
    public void Active(bool b)
    {
        SetVCamReference();
        vCam.enabled = b;
    }
}
