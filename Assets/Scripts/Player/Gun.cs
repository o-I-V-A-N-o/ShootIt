using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    [SerializeField, Space]
    private UiController UI;

    [SerializeField, Space]
    private Transform _projectilesSpawn;
    [SerializeField]
    private GameObject _projectilesPrefab;

    private float _horizontalAimDefault = 5f;
    private float _verticalAimDefault = 10f;

    [SerializeField, Space]
    private float _horizontalAim;
    [SerializeField]
    private float _verticalAim;

    [SerializeField, Space]
    private float shootSpeed = 1f;

    [SerializeField, Space]
    private int currentBullet;
    [SerializeField]
    private int maxBulletInGun = 20;
    [SerializeField]
    private int maxBullet = 100;

    private bool onShoot = false;
    private bool refreshing = false;

    private void Start()
    {
        _horizontalAim = _horizontalAimDefault;
        _verticalAim = _verticalAimDefault;

        currentBullet = maxBulletInGun;

        UI = _player.GetUI();
        UI.UIUpdateGun(currentBullet, maxBullet);
    }

    public void Shoot()
    {
        if (!onShoot)
        {
            if (currentBullet > 0)
            {
                onShoot = true;
                StartCoroutine(Shot());
            }
        }
    }

    public void Refresh()
    {
        if (!refreshing)
        {
            refreshing = true;
            StartCoroutine(RefreshGun());
        }
    }

    private IEnumerator Shot()
    {
        currentBullet -= 1;
        UI.UIUpdateGun(currentBullet, maxBullet);

        GameObject newProjectile = Instantiate(_projectilesPrefab);
        newProjectile.transform.position = _projectilesSpawn.position;
        newProjectile.transform.eulerAngles = _projectilesSpawn.eulerAngles + new Vector3(Random.Range(-_horizontalAim, _horizontalAim), 0f, Random.Range(-_verticalAim, _verticalAim));
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.up * 9999);
        newProjectile.GetComponent<ProjectilesController>().SetShooter(_player);

        yield return new WaitForSeconds(shootSpeed);

        onShoot = false;
        if (_player.automaticReload)
        {
            CheckGun();
        }
    }

    public void SetAiming(float horizontal, float vertical)
    {
        _horizontalAim = horizontal;
        _verticalAim = vertical;
    }

    public void SetAimingDefault()
    {
        _horizontalAim = _horizontalAimDefault;
        _verticalAim = _verticalAimDefault;
    }

    public void GetBullet(int bullets)
    {
        maxBullet += bullets;
        UI.UIUpdateGun(currentBullet, maxBullet);
    }

    private void CheckGun()
    {
        if (currentBullet <= 0)
        {
            refreshing = true;
            StartCoroutine(RefreshGun());
        }
    }

    private IEnumerator RefreshGun()
    {
        if (maxBullet > 0)
        {
            if (currentBullet >= 0 && currentBullet < maxBulletInGun)
            {
                int bullets = maxBulletInGun - currentBullet;
                if (maxBullet <= bullets)
                {
                    bullets = maxBullet;
                }
                currentBullet += bullets;
                maxBullet -= bullets;
            }
            else if (maxBullet < maxBulletInGun)
            {
                currentBullet = maxBullet;
                maxBullet = 0;
            }

            UI.UIUpdateGun(currentBullet, maxBullet);
        }
        yield return new WaitForSeconds(1f);
        refreshing = false;
    }

    public void TranslateGun(Transform startPos, Transform endPos)
    {
        StartCoroutine(Translate(startPos, endPos));
    }

    private IEnumerator Translate(Transform startPos, Transform endPos)
    {
        float translateSpeed = 5f;
        float translateDegree = 0f;
        while (translateDegree < 1)
        {
            this.transform.position = Vector3.Lerp(startPos.position, endPos.position, translateSpeed);
            translateDegree += translateSpeed * Time.deltaTime;
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void UpgradeShootSpeed()
    {
        if (shootSpeed > 0)
        {
            shootSpeed -= 0.05f;
        }
        else
        {
            shootSpeed = 0.1f;
        }
    }

    public void UpgradeAccuracy()
    {
        _horizontalAimDefault -= 0.25f;

        if (_horizontalAimDefault < 0)
        {
            _horizontalAim = 0f;
        }

        _verticalAimDefault -= 0.5f;

        if (_verticalAimDefault < 0)
        {
            _verticalAimDefault = 0f;
        }

        SetAimingDefault();
    }

    public int GetMaxBullet()
    {
        return maxBullet + currentBullet;
    }

    public void Activate()
    {
        UI.UIUpdateGun(currentBullet, maxBullet);
    }
}
