using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatusView : MonoBehaviour
{
    [SerializeField] private Slider _playerHpBar;
    [SerializeField] private Slider _enemyHpBar;
    [SerializeField] private TextMeshProUGUI _playerHpText;
    [SerializeField] private TextMeshProUGUI _enemyHpText;
    [SerializeField] private TextMeshProUGUI _cautionText;

    private const float MaxHp = 1000f;

    public void SetHp(float playerHp, float enemyHp)
    {
        _playerHpBar.maxValue = MaxHp;
        _enemyHpBar.maxValue = MaxHp;

        _playerHpBar.value = playerHp;
        _enemyHpBar.value = enemyHp;

        _playerHpText.text = playerHp.ToString("0");
        _enemyHpText.text = enemyHp.ToString("0");
    }

    public void SetCaution(int caution)
    {
        _cautionText.text = $"긴장도 : {caution}";
    }
}