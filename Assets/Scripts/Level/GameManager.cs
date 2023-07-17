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
    private Shop _playerShop;

    [SerializeField]
    private Renderer _floor;

    [SerializeField]
    private Renderer _wall;
    
    public bool inProgress = false;

    public int num = 0;

    private void Start()
    {
        StartCoroutine(ChangeColorMaterial());

        inProgress = true;
        OpenShop();
    }

    public void AddShootingRange(ShootingRangeController shootingRange)
    {
        _shootingRanges.Add(shootingRange);
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

    public void OpenShop()
    {
        _player.Shopping(true);
        Cursor.lockState = CursorLockMode.Confined;
        _playerShop.gameObject.SetActive(true);
        _playerShop.Open();
    }

    public void CloseShop()
    {
        _player.Shopping(false);
        Cursor.lockState = CursorLockMode.Locked;
        _playerShop.gameObject.SetActive(false);
        StartLevel(num);
    }

    public ShootingRangeController GetActiveShootingRange()
    {
        return _shootingRanges[num];
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

    public void StartRainbow()
    {
        StartCoroutine(ChangeColorMaterial());
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
