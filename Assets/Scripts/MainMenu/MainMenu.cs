using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetString("LoadGame", "no");
        PlayerPrefs.Save();
    }

    public void NewGame(string sceneName)
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Start new game");
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame()
    {
        PlayerPrefs.SetString("LoadGame", "yes");
        PlayerPrefs.Save();

        Debug.Log("Load savind game");
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Debug.Log("EXIT");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
