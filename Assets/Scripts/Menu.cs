using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;
    public float timeScale => menuUI.activeSelf ? 0.0001f : 1f;

    void Awake()
    {
        Continue();
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("esc");
            menuUI.SetActive(!menuUI.activeSelf);
            Time.timeScale = timeScale;
        }
        
    }

    public void Continue()
    {
        print("Called Continue");
        menuUI.SetActive(false);
        Time.timeScale = timeScale;
    }

    public void Reload()
    {
        print("Called Reload");
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Time.timeScale = timeScale;
    }

    public void Quit()
    {
        print("Called Quit");
        Time.timeScale = timeScale;
        Application.Quit();
    }
}
