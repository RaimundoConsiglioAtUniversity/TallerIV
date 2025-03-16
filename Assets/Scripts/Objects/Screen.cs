using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private PlayerCam cam;
    [SerializeField] private bool isStartScreen;
    public Vector3 screenSize = new(1, 1, 1);

    public void Width(int width) => screenSize.x = width;
    public void Height(int height) => screenSize.y = height;
    public void IsStartingScreen(bool startScreen) => isStartScreen = startScreen;
    public bool IsLargerThanFrustum => screenSize.x > 320 || screenSize.y > 180;
    public void SetDesiredScreenSize(Vector3 size)
    {
        Vector3 newSize = size / 16f;
        Vector3 frustumOffset = new(0.001f, 0.001f, 0f);

        if (IsLargerThanFrustum)
            newSize += frustumOffset;
            
        gameObject.transform.localScale = newSize;
    }

#if UNITY_EDITOR

    [UnityEditor.Callbacks.DidReloadScripts]
    public static void SetScreenSize()
    {
        Screen[] screens = FindObjectsOfType<Screen>();
    
        foreach (var screen in screens)
        {
            screen.SetDesiredScreenSize(screen.screenSize);

            if (screen.isStartScreen)
                screen.cam.Enable();
                
            else
                screen.cam.Disable();
        }
    }
    
    #endif

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != PlayerInput.Instance.screenTrigger)
            return;

        cam.Enable();
    }

    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col != PlayerInput.Instance.screenTrigger)
            return;

        cam.Disable();
    }
}
