using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private ShootingRangeController _shootingRangeController;

    private PlayerController _killer;

    private int _id;

    private void Update()
    {
        var x = Random.Range(0f, 20f);
        var y = Random.Range(0f, 20f);
        var z = Random.Range(0f, 20f);
        transform.Rotate(new Vector3(x, y, z) * 10f * Time.deltaTime);
    }

    public void SetId(int id)
    {
        _id = id;
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

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Enemy-" + _id, +_id);

        PlayerPrefs.SetFloat("EnemyPosX-" + _id, transform.position.x);
        PlayerPrefs.SetFloat("EnemyPosY-" + _id, transform.position.y);
        PlayerPrefs.SetFloat("EnemyPosZ-" + _id, transform.position.z);

        PlayerPrefs.SetFloat("EnemyRotX-" + _id, transform.rotation.x);
        PlayerPrefs.SetFloat("EnemyRotY-" + _id, transform.rotation.y);
        PlayerPrefs.SetFloat("EnemyRotZ-" + _id, transform.rotation.z);
        Debug.Log("Save - enemy-" + _id);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Enemy-" + _id))
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("EnemyPosX-" + _id), PlayerPrefs.GetFloat("EnemyPosY-" + _id), PlayerPrefs.GetFloat("EnemyPosZ-" + _id));
            transform.eulerAngles = new Vector3(PlayerPrefs.GetFloat("EnemyRotX-" + _id), PlayerPrefs.GetFloat("EnemyRotY-" + _id), PlayerPrefs.GetFloat("EnemyRotZ-" + _id));
            Debug.Log("Load - enemy-" + _id);
        }
        else
        {
            Destroy(this.gameObject);
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
