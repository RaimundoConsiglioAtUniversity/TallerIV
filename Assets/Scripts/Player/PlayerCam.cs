using UnityEngine;
using Cinemachine;
public class PlayerCam : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    bool follow = false;

    void Start()
    {
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (!follow)
            return;
        
        vCam.Follow = PlayerInput.Instance.pony.transform;
    }

    public void doesFollow(bool value) => follow = value;
    public void Enable() => vCam.enabled = true;
    public void Disable() => vCam.enabled = false;
}
