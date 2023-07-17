using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private ShootingRangeController _shootingRangeController;

    private PlayerController _killer;

    public void SetShootingRangeController(ShootingRangeController shootingRangeController)
    {
        _shootingRangeController = shootingRangeController;
    }

    public void SetKiller(PlayerController killer)
    {
        _killer = killer;
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
            else
            {
                _killer.GetDamage(20);
            }

            switch (partBody)
            { 
                case "Simple":
                    _killer.GetScore(1);
                    break;
                case "Critical":
                    _killer.GetScore(2);
                    break;
            }
        }
        _shootingRangeController.RemoveEnemy(this);

        Destroy(this.gameObject);
    }
}
