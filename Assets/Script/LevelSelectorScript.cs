using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorScript : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().PlayMusic();
    }
    public void Level1()
    {
        SceneManager.LoadScene(2);
    }
    public void Level2()
    {
        SceneManager.LoadScene(3);
    }
    public void Level3()
    {
        SceneManager.LoadScene(4);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>().StopMusic();
        Destroy(GameObject.FindGameObjectWithTag("Music"));
    }
}
