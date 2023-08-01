using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField, Space]
    private Transform [] _enemyPool;

    [SerializeField]
    private List<EnemyController> _enemies = new List<EnemyController>();

    [SerializeField]
    private GameObject[] _enemyPrefabs;

    [SerializeField, Space]
    private Transform _gate;

    [SerializeField, Space]
    private Transform _playerPosition;

    private int _id;

    private bool _loaded = false;


    public void SetId(int id)
    {
        _id = id;
    }
    public void SetEnemy(Transform enemy)
    {
        foreach (Transform enemyPosition in _enemyPool)
        {
            if (enemyPosition.childCount == 0)
            {
                enemy.parent = enemyPosition;
                enemy.position = enemyPosition.position;
                enemy.parent.rotation = enemyPosition.rotation;
            }
        }
    }

    void Start()
    {
        _gameManager = transform.parent.GetComponent<GameManager>();
        _gameManager.AddShootingRange(this);
    }

    private void AddEnemy()
    {
        for (int i = 0; i < _enemyPool.Length; i++)
        {
            GameObject newEnemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
            SetEnemy(newEnemy.transform);
            newEnemy.GetComponent<EnemyController>().SetShootingRangeController(this);
            newEnemy.GetComponent<EnemyController>().SetId(i);
            _enemies.Add(newEnemy.GetComponent<EnemyController>());

            if (_loaded)
            {
                newEnemy.GetComponent<EnemyController>().LoadGame();
            }
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
        CheckEnemyCount();
    }

    private void CheckEnemyCount()
    {
        int countEnemy = 0;
        foreach (var enemy in _enemies)
        {
            if (enemy != null)
            {
                countEnemy++;
            }
        }

        if (countEnemy < 1)
        {
            EndLevel();
        }
    }

    private void EndLevel()
    {
        StartCoroutine(CloseGate());
    }

    public void Begin()
    {
        AddEnemy();
        StartCoroutine(OpenGate());
    }

    private IEnumerator OpenGate()
    {
        float speed = -10f;

        while (_gate.position.y > -24f)
        {
            _gate.Translate(0f, speed * Time.deltaTime, 0f);

            yield return null;
        }
    }

    private IEnumerator CloseGate()
    {
        float speed = 10f;

        while (_gate.position.y < 25f)
        {
            _gate.Translate(0f, speed * Time.deltaTime, 0f);

            yield return null;
        }

        _gameManager.NextLevel();
    }

    public Transform GetPlayerPosition()
    {
        return _playerPosition;
    }

    public List<EnemyController> GetEnemies()
    {
        return _enemies;
    }

    public void SaveGame()
    {
        if (_enemies.Count > 0)
        {
            foreach (EnemyController enemy in _enemies)
            {
                if (enemy != null)
                {
                    enemy.SaveGame();
                }
            }
        }
        Debug.Log("Save - shooting range " + _id);
    }

    public void LoadGame()
    {
        _loaded = true;
        Debug.Log("Load - shooting range " + _id);
    }
}
