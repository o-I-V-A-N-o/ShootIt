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
    private int _machineGunAccuracyLevel = 0;

    private int _pistolSpeedLevel = 0;
    private int _pistolAccuracyLevel = 0;

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
        if (_player.GetScoreBalance(price))
        {
            _machineGunSpeedLevel++;
            _player.UpgradeGun("MachineGun", "Speed");
            UpdateUI(_uiMachineGunSpeedLevel, _machineGunSpeedLevel.ToString());
            UpdateUIScoreBalance();
        }
    }

    public void UpgradeMGAccuracyLevel()
    {
        if (_player.GetScoreBalance(price))
        {
            _machineGunAccuracyLevel++;
            _player.UpgradeGun("MachineGun", "Accuracy");
            UpdateUI(_uiMachineGunAccuracyLevel, _machineGunAccuracyLevel.ToString());
            UpdateUIScoreBalance();
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
        if (_player.GetScoreBalance(price))
        {
            _pistolSpeedLevel++;
            _player.UpgradeGun("Pistol", "Speed");
            UpdateUI(_uiPistolSpeedLevel, _pistolSpeedLevel.ToString());
            UpdateUIScoreBalance();
        }
    }

    public void UpgradePAccuracyLevel()
    {
        if (_player.GetScoreBalance(price))
        {
            _pistolAccuracyLevel++;
            _player.UpgradeGun("Pistol", "Accuracy");
            UpdateUI(_uiPistolAccuracyLevel, _pistolAccuracyLevel.ToString());
            UpdateUIScoreBalance();
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
            UpdateUI(_uiAutomaticReload, "V");
            UpdateUIScoreBalance();
        }
    }

    private void UpdateUIScoreBalance()
    {
        _uiScoreCount.text = _player.GetScoreBalance().ToString();
    }
}
