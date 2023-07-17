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

    void Update()
    {
        
    }

    private void AddEnemy()
    {
        for (int i = 0; i < _enemyPool.Length; i++)
        {
            GameObject newEnemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]);
            SetEnemy(newEnemy.transform);
            newEnemy.GetComponent<EnemyController>().SetShootingRangeController(this);
            _enemies.Add(newEnemy.GetComponent<EnemyController>());
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        _enemies.Remove(enemy);
        CheckEnemyCount();
    }

    private void CheckEnemyCount()
    {
        if (_enemies.Count < 1)
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
}
