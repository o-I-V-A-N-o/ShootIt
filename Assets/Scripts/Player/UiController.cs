using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentBullet;
    [SerializeField]
    private TextMeshProUGUI _maxBullet;
    [SerializeField]
    private GameObject _activeGunImage;
    [SerializeField]
    private Sprite [] _gunsImage;

    [SerializeField, Space]
    private TextMeshProUGUI _health;

    [SerializeField, Space]
    private TextMeshProUGUI _currentShootingRange;
    [SerializeField]
    private TextMeshProUGUI _maxShootingRange;

    [SerializeField, Space]
    private TextMeshProUGUI _scoreCurrent;
    [SerializeField]
    private TextMeshProUGUI _scoreRecord;
    [SerializeField]
    private TextMeshProUGUI _scoreBalance;

    [SerializeField, Space]
    private TextMeshProUGUI _medicinePack;

    [SerializeField]
    private TextMeshProUGUI _grenadePack;

    public void UIUpdateGun(int current, int max)
    {
        _currentBullet.text = current.ToString();
        _maxBullet.text = max.ToString();
    }

    public void UIUpdateProgress(int current, int max)
    {
        _currentShootingRange.text = current.ToString();
        _maxShootingRange.text = max.ToString();
    }

    public void UIUpdateScoreCurrent(int score)
    {
        _scoreCurrent.text = score.ToString();
    }

    public void UIUpdateScoreRecord(int score)
    {
        _scoreRecord.text = score.ToString();
    }

    public void UIUpdateScoreBalance(int score)
    {
        _scoreBalance.text = score.ToString();
    }

    public void UIUpdateMedicine(int medicine)
    {
        _medicinePack.text = medicine.ToString();
    }

    public void UIUpdateGrenade(int grenade)
    {
        _grenadePack.text = grenade.ToString();
    }

    public void UIUpdateHealth(int health)
    {
        _health.text = health.ToString();
    }

    public void SetGunImage(int num)
    {
        _activeGunImage.GetComponent<Image>().sprite = _gunsImage[num];
    }
}
