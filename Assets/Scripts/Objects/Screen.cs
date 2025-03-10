using UnityEngine;

public class Screen : MonoBehaviour
{
    private PlayerCam cam;
    public Vector3 screenSize = new(1, 1, 1);

    public void Width(int width) => screenSize.x = width;
    public void Height(int height) => screenSize.y = height;
    public bool isLargerThanFrustrum => (screenSize.x > 320 || screenSize.y > 180) ? true : false;


    public void GetDesiredScreenSize(Vector3 size)
    {
        gameObject.transform.localScale = size / 16f;
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    public static void SetScreenSize()
    {
        Screen[] screens = FindObjectsOfType<Screen>();

        foreach (var screen in screens)
        {
            screen.GetDesiredScreenSize(screen.screenSize);
            screen.cam = screen.GetComponent<PlayerCam>();
            screen.cam.doesFollow(screen.isLargerThanFrustrum);
            screen.cam.Disable();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != PlayerInput.Instance.pony.col)
            return;

        cam.Enable();
        print($"Camera {cam.gameObject.name} should be enabled");
    }

    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col != PlayerInput.Instance.pony.col)
            return;

        cam.Disable();
        print($"Camera {cam.gameObject.name} should be disabled");
    }
}
