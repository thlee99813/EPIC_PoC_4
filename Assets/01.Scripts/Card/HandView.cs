using TMPro;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private TextMeshProUGUI _remainingCountText;

    public void SetText(string text)
    {
        _valueText.text = text;
    }

    public void SetRemainingCount(int count)
    {
        _remainingCountText.text = $"남은 횟수 : {count}";
    }
}