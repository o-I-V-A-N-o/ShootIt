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

    [SerializeField]
    private GameObject _particle;
    [SerializeField]
    private GameObject _particleBoom;

    void Start()
    {
        StartCoroutine(LifeTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        var newParticle = Instantiate(_particle);
        newParticle.transform.position = transform.position;

        if (_explode)
        {
            Explode();
            var newParticleBoom = Instantiate(_particleBoom);
            newParticleBoom.transform.position = transform.position;
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
        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, _explodeRadius);

        for (int i = 0; i < overlapColliders.Length; i++)
        {
            if (overlapColliders[i].tag == "Enemy")
            {
                var enemy = overlapColliders[i].transform.parent.GetComponent<EnemyController>();
                enemy.SetKiller(_shooter);
                enemy.Die("Simple");
            }
        }
    }
}
