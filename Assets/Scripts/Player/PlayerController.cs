using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameObject player;

    [SerializeField, Space]
    private CameraController playerCamera;

    [SerializeField]
    private int speed = 5;
    [SerializeField]
    private float rotateSpeed = 50f;
    [SerializeField]
    private int _maxHealth = 100;
    private int _health;

    [SerializeField, Space]
    private Gun[] _guns;
    [SerializeField]
    private Gun _activeGun;
    private int _activeGunNum = 0;
    [SerializeField]
    private Transform _activeGunPosition;
    [SerializeField]
    private Transform _activeGunAimPosition;
    [SerializeField]
    private Transform _inactiveGunPosition;

    [SerializeField, Space]
    private GameObject _projectilesPrefab;
    
    [SerializeField, Space]
    private UiController UI;

    [SerializeField]
    private int _scoreCurrent = 0;
    [SerializeField]
    private int _scoreBalance = 0;
    [SerializeField]
    private int _scoreRecord = 0;

    [SerializeField, Space]
    private int medicineHealth = 50;
    private int medicine = 1;
    private bool _useMedicine = false;

    [SerializeField, Space]
    private Transform _grenadeSpawn;
    private int grenade = 5;
    private bool _useGrenade = false;

    public bool automaticReload = false;

    public bool isShopping = false;

    void Start()
    {
        _health = _maxHealth;
        UI.UIUpdateHealth(_health);
        UI.UIUpdateGrenade(grenade);
        UI.UIUpdateMedicine(medicine);
        _scoreBalance = PlayerPrefs.GetInt("ScoreBalance");
        _scoreRecord = PlayerPrefs.GetInt("Score");
        UI.UIUpdateScoreCurrent(_scoreCurrent);
        UI.UIUpdateScoreRecord(_scoreRecord);
        UI.UIUpdateScoreBalance(_scoreBalance);
    }

    void Update()
    {
        if (!isShopping)
        {
            //Move();
            UseGun();
            Aiming();
            UseInventory();
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.position += player.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.position -= player.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //player.transform.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
            player.transform.position += player.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //player.transform.Rotate(new Vector3(0f, -rotateSpeed * Time.deltaTime, 0f));
            player.transform.position -= player.transform.right * speed * Time.deltaTime;
        }
    }

    private void UseGun()
    {
        if (Input.GetMouseButton(0))
        {
            _activeGun.Shoot();
        }

        if (Input.GetKey(KeyCode.R))
        {
            _activeGun.Refresh();
        }

        float mouse = Input.GetAxis("Mouse ScrollWheel");
        if (mouse > 0.1)
        {
            if (_activeGunNum >= _guns.Length - 1)
            {
                _activeGunNum = 0;
            }
            else
            {
                _activeGunNum += 1;
            }
            Debug.Log(_activeGunNum + "" + _guns.Length);
            SetActiveGun(_activeGunNum);
        }
        if (mouse < -0.1)
        {
            if (_activeGunNum > 0)
            {
                _activeGunNum -= 1;
            }
            else
            {
                _activeGunNum = _guns.Length - 1;
            }
            Debug.Log(_activeGunNum + "" + _guns.Length);
            SetActiveGun(_activeGunNum);
        }
    }

    private void Aiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _activeGun.TranslateGun(_activeGun.transform, _activeGunAimPosition);
            _activeGun.SetAiming(0f, 0f);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _activeGun.TranslateGun(_activeGun.transform, _activeGunPosition);
            _activeGun.SetAimingDefault();
        }
    }

    private void UseInventory()
    {
        if (!_useGrenade & Input.GetKey(KeyCode.E))
        {
            if (grenade > 0)
            {
                _useGrenade = true;
                StartCoroutine(UseGrenade());
            }
        }
        if (!_useMedicine & Input.GetKey(KeyCode.Q))
        {
            if (medicine > 0)
            {
                _useMedicine = true;
                StartCoroutine(UseMedicine());
            }
        }
    }

    public void UpdateProgress(int current, int max)
    {
        UI.UIUpdateProgress(current, max);
    }

    public void GetMedicine(int medPack)
    {
        medicine += medPack;
        UI.UIUpdateMedicine(medicine);
    }

    private IEnumerator UseMedicine()
    {
        medicine -= 1;
        GetHealth(medicineHealth);
        
        UI.UIUpdateMedicine(medicine);
        yield return new WaitForSeconds(1f);
        _useMedicine = false;
    }

    public void GetGrenade(int grenadePack)
    {
        grenade += grenadePack;
        UI.UIUpdateGrenade(grenade);
    }

    private IEnumerator UseGrenade()
    {
        grenade -= 1;
        UI.UIUpdateGrenade(grenade);
        GameObject newGrenade = Instantiate(_projectilesPrefab);
        newGrenade.transform.position = _grenadeSpawn.position;
        newGrenade.transform.rotation = _grenadeSpawn.rotation;
        newGrenade.GetComponent<Rigidbody>().AddForce(newGrenade.transform.up * 2222);
        newGrenade.GetComponent<ProjectilesController>().SetGameManager(_gameManager);
        newGrenade.GetComponent<ProjectilesController>().SetShooter(this);
        newGrenade.GetComponent<ProjectilesController>().SetExplode(true);
        yield return new WaitForSeconds(1f);
        _useGrenade = false;
    }

    public void GetBullet(int bullets)
    {
        _activeGun.GetBullet(bullets);
    }

    public void GetDamage(int damage)
    {
        _health -= damage;
        UI.UIUpdateHealth(_health);
        if (_health <= 0)
        {
            _gameManager.EndGame();
        }
    }

    public void GetHealth(int health)
    {
        _health += health;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        UI.UIUpdateHealth(_health);
    }

    public void GetScore(int score)
    {
        _scoreCurrent += score;
        UI.UIUpdateScoreCurrent(_scoreCurrent);

        if (_scoreCurrent > _scoreRecord)
        {
            _scoreRecord = _scoreCurrent;
            UI.UIUpdateScoreRecord(_scoreRecord);
            SaveScore();
        }
        if (GetScoreBalance(score))
        {
            SaveScoreBalance();
        }
    }

    public bool GetScoreBalance(int score)
    {
        if (score > 0 || _scoreBalance > 0)
        {
            _scoreBalance += score;
            SaveScoreBalance();

            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetScoreBalance()
    {
        Debug.Log(_scoreBalance);
        return _scoreBalance;
    }

    public void UpgradeGun(string gun, string parametr)
    {
        if(gun == "Pistol")
        {
            switch (parametr)
            {
                case "Speed":
                    _guns[0].UpgradeShootSpeed();
                    break;
                case "Accuracy":
                    _guns[0].UpgradeAccuracy();
                    break;
                case "Ammo":
                    _guns[0].GetBullet(10);
                    break;
            }
        }
        else if (gun == "MachineGun")
        {
            switch (parametr)
            {
                case "Speed":
                    _guns[1].UpgradeShootSpeed();
                    break;
                case "Accuracy":
                    _guns[1].UpgradeAccuracy();
                    break;
                case "Ammo":
                    _guns[1].GetBullet(10);
                    break;
            }
        }
    }

    public int GetCountBullet(string gun)
    {
        int bullet = 0;
        
        switch(gun)
        {
            case "Pistol":
                bullet = _guns[0].GetMaxBullet();
                break;
            case "MachineGun":
                bullet = _guns[1].GetMaxBullet();
                break;
        }

        return bullet;
    }

    private void SetActiveGun(int number)
    {
        _activeGun.transform.position = _inactiveGunPosition.position;
        _activeGun.transform.parent = _inactiveGunPosition;

        _activeGun = _guns[number];

        UI.SetGunImage(number);
        _activeGun.Activate();
        _activeGun.transform.position = _activeGunPosition.position;
        _activeGun.transform.rotation = _activeGunPosition.rotation;
        _activeGun.transform.parent = _activeGunPosition;
    }

    public void Shopping(bool state)
    {
        isShopping = state;
        playerCamera.LockCamera(state);
    }

    public UiController GetUI()
    {
        return UI;
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Score", _scoreRecord);
    }

    public void SaveScoreBalance()
    {
        UI.UIUpdateScoreBalance(_scoreBalance);
        PlayerPrefs.SetInt("ScoreBalance", _scoreBalance);
    }
}
