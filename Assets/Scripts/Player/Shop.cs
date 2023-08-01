using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    [SerializeField, Space]
    private UiController UI;

    [SerializeField, Space]
    private TextMeshProUGUI _uiScoreCount;

    [SerializeField, Space]
    private TextMeshProUGUI _uiMachineGunSpeedLevel;
    [SerializeField]
    private TextMeshProUGUI _uiMachineGunAccuracyLevel;
    [SerializeField]
    private TextMeshProUGUI _uiMachineGunAmmo;

    [SerializeField, Space]
    private TextMeshProUGUI _uiPistolSpeedLevel;
    [SerializeField]
    private TextMeshProUGUI _uiPistolAccuracyLevel;
    [SerializeField]
    private TextMeshProUGUI _uiPistolAmmo;

    [SerializeField, Space]
    private TextMeshProUGUI _uiAutomaticReload;

    private int _machineGunSpeedLevel = 0;
    private int _machineGunSpeedLevelMax = 20;
    private int _machineGunAccuracyLevel = 0;
    private int _machineGunAccuracyLevelMax = 20;

    private int _pistolSpeedLevel = 0;
    private int _pistolSpeedLevelMax = 20;
    private int _pistolAccuracyLevel = 0;
    private int _pistolAccuracyLevelMax = 20;

    private bool _automaticReload;
    private int price = -1;

    public void Open()
    {
        _uiPistolAmmo.text = _player.GetCountBullet("Pistol").ToString();
        _uiMachineGunAmmo.text = _player.GetCountBullet("MachineGun").ToString();
        _uiScoreCount.text = _player.GetScoreBalance().ToString();
    }

    private void UpdateUI(TextMeshProUGUI component, string value)
    {
        component.text = value;
    }

    public void UpgradeMGSpeedLevel()
    {
        if (_machineGunSpeedLevel < _machineGunSpeedLevelMax)
        {
            if (_player.GetScoreBalance(price))
            {
                _machineGunSpeedLevel++;
                _player.UpgradeGun("MachineGun", "Speed");
                UpdateUI(_uiMachineGunSpeedLevel, _machineGunSpeedLevel.ToString());
                UpdateUIScoreBalance();
            }
        }
    }

    public void UpgradeMGAccuracyLevel()
    {
        if (_machineGunAccuracyLevel < _machineGunAccuracyLevelMax)
        {
            if (_player.GetScoreBalance(price))
            {
                _machineGunAccuracyLevel++;
                _player.UpgradeGun("MachineGun", "Accuracy");
                UpdateUI(_uiMachineGunAccuracyLevel, _machineGunAccuracyLevel.ToString());
                UpdateUIScoreBalance();
            }
        }
    }

    public void UpgradeMGAmmo()
    {
        if (_player.GetScoreBalance(price))
        {
            _player.UpgradeGun("MachineGun", "Ammo");
            UpdateUI(_uiMachineGunAmmo, _player.GetCountBullet("MachineGun").ToString());
            UpdateUIScoreBalance();
        }
    }

    public void UpgradePSpeedLevel()
    {
        if (_pistolSpeedLevel < _pistolSpeedLevelMax)
        {
            if (_player.GetScoreBalance(price))
            {
                _pistolSpeedLevel++;
                _player.UpgradeGun("Pistol", "Speed");
                UpdateUI(_uiPistolSpeedLevel, _pistolSpeedLevel.ToString());
                UpdateUIScoreBalance();
            }
        }
    }

    public void UpgradePAccuracyLevel()
    {
        if (_pistolAccuracyLevel < _pistolAccuracyLevelMax)
        {
            if (_player.GetScoreBalance(price))
            {
                _pistolAccuracyLevel++;
                _player.UpgradeGun("Pistol", "Accuracy");
                UpdateUI(_uiPistolAccuracyLevel, _pistolAccuracyLevel.ToString());
                UpdateUIScoreBalance();
            }
        }
    }

    public void UpgradePAmmo()
    {
        if (_player.GetScoreBalance(price))
        {
            _player.UpgradeGun("Pistol", "Ammo");
            UpdateUI(_uiPistolAmmo, _player.GetCountBullet("Pistol").ToString());
            UpdateUIScoreBalance();
        }
    }

    public void SetAutomaticReloadGun()
    {
        if (!(_player.automaticReload == true) && _player.GetScoreBalance(price))
        {
            _player.automaticReload = true;
            _automaticReload = true;
            UpdateUI(_uiAutomaticReload, "V");
            UpdateUIScoreBalance();
        }
    }

    private void UpdateUIScoreBalance()
    {
        _uiScoreCount.text = _player.GetScoreBalance().ToString();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("MachineGunSpeedLevel", _machineGunSpeedLevel);
        PlayerPrefs.SetInt("MachineGunAccuracyLevel", _machineGunAccuracyLevel);

        PlayerPrefs.SetInt("PistolSpeedLevel", _pistolSpeedLevel);
        PlayerPrefs.SetInt("PistolAccuracyLevel", _pistolAccuracyLevel);

        if (_automaticReload)
        {
            PlayerPrefs.SetInt("AutomaticReload", 1);
        }

        Debug.Log("Save - shop");
    }

    public void LoadGame()
    {
        _machineGunSpeedLevel = PlayerPrefs.GetInt("MachineGunSpeedLevel");
        UpdateUI(_uiMachineGunSpeedLevel, _machineGunSpeedLevel.ToString());
        _machineGunAccuracyLevel = PlayerPrefs.GetInt("MachineGunAccuracyLevel");
        UpdateUI(_uiMachineGunAccuracyLevel, _machineGunAccuracyLevel.ToString());

        _pistolSpeedLevel = PlayerPrefs.GetInt("PistolSpeedLevel");
        UpdateUI(_uiPistolSpeedLevel, _pistolSpeedLevel.ToString());
        _pistolAccuracyLevel = PlayerPrefs.GetInt("PistolAccuracyLevel");
        UpdateUI(_uiPistolAccuracyLevel, _pistolAccuracyLevel.ToString());

        if (PlayerPrefs.GetInt("AutomaticReload") > 0)
        {
            _automaticReload = true;
            _player.automaticReload = true;
            UpdateUI(_uiAutomaticReload, "V");
        }

        Debug.Log("Load - shop");
    }
    
}
