using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private ShootingRangeController _shootingRangeController;

    private PlayerController _killer;

    private void Update()
    {
        var x = Random.Range(0f, 20f);
        var y = Random.Range(0f, 20f);
        var z = Random.Range(0f, 20f);
        transform.Rotate(new Vector3(x, y, z) * 10f * Time.deltaTime);
        //StartCoroutine(Rotate());
    }

    public void SetShootingRangeController(ShootingRangeController shootingRangeController)
    {
        _shootingRangeController = shootingRangeController;
    }

    public void SetKiller(PlayerController killer)
    {
        _killer = killer;
    }

    private IEnumerator Rotate()
    {
        float rotateSpeed = Random.Range(0f, 10f);

        yield return null;
        
        while (true)
        {
            transform.Rotate(new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * Time.deltaTime);
        }
    }

    public void Die(string partBody)
    {
        if (_killer != null)
        {
            float random = Random.Range(0f, 100f);
            if (random > 70)
            {
                _killer.GetGrenade(1);
            }
            else if (random > 50)
            {
                _killer.GetMedicine(1);
            }
            else if (random > 20)
            {
                _killer.GetBullet(20);
            }
            else if (random > 10)
            {
                _killer.LowAccuracyAndFastSpeed();
            }
            else
            {
                _killer.GetDamage(20);
            }

            switch (partBody)
            { 
                case "Simple":
                    _killer.GetScore(2);
                    break;
                case "Critical":
                    _killer.GetScore(4);
                    break;
            }
        }
        _shootingRangeController.RemoveEnemy(this);

        Destroy(this.gameObject);
    }
}
