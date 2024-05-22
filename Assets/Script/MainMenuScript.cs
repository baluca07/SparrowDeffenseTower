using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
    }

    public void LevelSelector()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitApp() { 
        Application.Quit();
    }
}
