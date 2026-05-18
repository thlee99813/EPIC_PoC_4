using TMPro;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;

    public void SetText(string text)
    {
        _valueText.text = text;
    }
}