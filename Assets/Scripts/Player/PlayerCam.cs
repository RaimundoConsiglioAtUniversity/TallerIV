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

    public void Enable()
    {
        SetVCamReference();
        vCam.enabled = true;
    }
    
    public void Disable()
    {
        SetVCamReference();
        vCam.enabled = false;
    }
}
