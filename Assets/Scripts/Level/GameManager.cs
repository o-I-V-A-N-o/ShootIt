using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<ShootingRangeController> _shootingRanges = new List<ShootingRangeController>();

    [SerializeField]
    private PlayerController _player;

    [SerializeField]
    private GameObject _gameUI;

    [SerializeField]
    private Shop _playerShop;

    [SerializeField]
    private Renderer _floor;

    [SerializeField]
    private Renderer _wall;
    
    public bool inProgress = false;

    private bool inPause = false;

    public int num = 0;

    private int _srId = 0;

    private void Start()
    {
        StartCoroutine(ChangeColorMaterial());
        if (PlayerPrefs.GetString("LoadGame") == "yes")
        {
            LoadGame();
        }
        inProgress = true;
        OpenShop();
    }

    public void AddShootingRange(ShootingRangeController shootingRange)
    {
        _shootingRanges.Add(shootingRange);
        shootingRange.SetId(_srId);
        _srId++;
    }

    private void StartLevel(int number)
    {
        Transform playerPos = _shootingRanges[number].GetPlayerPosition();
        _player.transform.position = playerPos.position;
        _player.transform.rotation = playerPos.rotation;
        
        _player.UpdateProgress(num, _shootingRanges.Count);
        
        _shootingRanges[number].Begin();
    }

    public void NextLevel()
    {
        num++;
        if (num < _shootingRanges.Count)
        {
            OpenShop();
        }
        else
        {
            EndGame();
        }
        
    }

    public void PauseGame()
    {
        if (inPause)
        {
            Time.timeScale = 1f;
            _gameUI.SetActive(false);
            _player.Stopped(false, "pause");
            inPause = false;
            if (!_player.isShopping)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            Time.timeScale = 0f;
            _gameUI.SetActive(true);
            _player.Stopped(true, "pause");
            inPause = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void OpenShop()
    {
        _player.Stopped(true, "shop");
        Cursor.lockState = CursorLockMode.Confined;
        _playerShop.gameObject.SetActive(true);
        _playerShop.Open();
    }

    public void CloseShop()
    {
        _player.Stopped(false, "shop");
        Cursor.lockState = CursorLockMode.Locked;
        _playerShop.gameObject.SetActive(false);
        StartLevel(num);
    }

    public ShootingRangeController GetActiveShootingRange()
    {
        return _shootingRanges[num];
    }

    public void SaveGame()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Start save game !");
        PlayerPrefs.SetInt("Game_progress", num);
        _shootingRanges[num].SaveGame();
        _player.SaveGame();
        _playerShop.SaveGame();
        PlayerPrefs.Save();
        Debug.Log("Save");
    }

    public void LoadGame()
    {
        num = PlayerPrefs.GetInt("Game_progress");
        _srId = num;
        _shootingRanges[num].LoadGame();
        _player.LoadGame();
        _playerShop.LoadGame();
        Debug.Log("Load");
    }

    public void EndGame()
    {
        Debug.Log("Конец игры!");

        _player.SaveScore();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if PLATFORM_STANDALONE_WIN
        Application.Quit();
#endif
    }

    private IEnumerator ChangeColorMaterial()
    {
        float r = 1;
        float g = 0;
        float b = 1;
        bool red = true;
        bool green = false;
        bool blue = false;
        float startSpeed = 0.01f;
        float speed = startSpeed;

        while (true)
        {
            if (red)
            {
                if (r > 1f)
                {
                    speed = -startSpeed;
                }
                else if (r < 0.01f)
                {
                    speed = startSpeed;
                }

                r += speed;
                if (r > 1f || r < 0.01f)
                {
                    red = false;
                    green = true;
                }
            }

            if (green)
            {
                if (g > 1f)
                {
                    speed = -startSpeed;
                }
                else if (g < 0.01f)
                {
                    speed = startSpeed;
                }

                g += speed;
                if (g > 1f || g < 0.01f)
                {
                    green = false;
                    blue = true;
                }
            }

            if (blue)
            {
                if (b > 1f)
                {
                    speed = -startSpeed;
                }
                else if (b < 0.01f)
                {
                    speed = startSpeed;
                }

                b += speed;
                if (b > 1f || b < 0.01f)
                {
                    blue = false;
                    red = true;
                }
            }
            yield return new WaitForSeconds(0.01f);

            _wall.sharedMaterial.SetColor("_Color", new Color(r, g, b));
            _floor.material.SetColor("_Color", new Color(g, r, b));
        }
    }
}
