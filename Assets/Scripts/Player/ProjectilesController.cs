using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private float _speed = 50f;

    [SerializeField]
    private float _lifeTime = 5f;

    private PlayerController _shooter;

    private bool _explode = false;
    [SerializeField]
    private float _explodeRadius = 30f;

    [SerializeField]
    private List<EnemyController> _enemies;

    void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_explode)
        {
            Explode();
        }
        else if (collision.transform.parent.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemy.SetKiller(_shooter);
            enemy.Die(collision.transform.name);
        }

        Destroy(this.gameObject);
    }

    public void SetGameManager(GameManager manager)
    {
        _gameManager = manager;
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(this.gameObject);
    }

    public void SetShooter(PlayerController shooter)
    {
        _shooter = shooter;
    }

    public void SetExplode(bool state)
    {
        _explode = state;
    }

    public void Explode()
    {
        _enemies = _gameManager.GetActiveShootingRange().GetEnemies();
        
        for (int i = 0; i < _enemies.Count; i++)
        {
            float distance = Vector3.SqrMagnitude(this.transform.position - _enemies[i].transform.position);

            if (distance < _explodeRadius * _explodeRadius)
            {
                _enemies[i].SetKiller(_shooter);
                _enemies[i].Die("Simple");
            }
        }
    }
}
