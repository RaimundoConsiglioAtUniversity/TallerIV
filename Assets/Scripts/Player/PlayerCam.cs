using UnityEngine;
using Cinemachine;
public class PlayerCam : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        cam.Follow = PlayerInput.Instance.pony.transform;
    }
}
