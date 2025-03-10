using UnityEngine;

public class Screen : MonoBehaviour
{
    public Vector3 screenSize = new(1, 1, 1);

    public void Width(int width) => screenSize.x = width;
    public void Height(int height) => screenSize.y = height;


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
        }
    }
}
