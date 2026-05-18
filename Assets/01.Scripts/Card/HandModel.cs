using UnityEngine;

public class HandModel
{
    private const float MinValue = -1000f;
    private const float MaxValue = 1000f;
    private const float StartValue = 1f;
    private const int MaxOperationCount = 3;

    private float _currentValue = StartValue;
    private string _pendingSymbol;
    private int _usedOperationCount;

    public float CurrentValue => _currentValue;
    public bool NeedsSymbol => string.IsNullOrEmpty(_pendingSymbol);
    public int RemainingOperationCount => MaxOperationCount - _usedOperationCount;
    public bool CanStartOperation => RemainingOperationCount > 0;

    public bool TryAdd(string value, DraftCardType cardType)
    {
        if (NeedsSymbol && cardType != DraftCardType.Symbol)
        {
            return false;
        }

        if (!NeedsSymbol && cardType != DraftCardType.Number)
        {
            return false;
        }

        if (cardType == DraftCardType.Symbol)
        {
            if (!CanStartOperation)
            {
                return false;
            }

            _pendingSymbol = value;
            return true;
        }

        float number = float.Parse(value);
        ApplyNumber(number);
        _pendingSymbol = null;
        _usedOperationCount++;
        return true;
    }

    public void Reset()
    {
        _currentValue = StartValue;
        _pendingSymbol = null;
        _usedOperationCount = 0;
    }

    private void ApplyNumber(float number)
    {
        if (_pendingSymbol == "+")
        {
            _currentValue += number;
        }
        else if (_pendingSymbol == "-")
        {
            _currentValue -= number;
        }
        else if (_pendingSymbol == "\u00D7")
        {
            _currentValue *= number;
        }
        else if (_pendingSymbol == "\u00F7")
        {
            _currentValue /= number;
        }

        _currentValue = Mathf.Clamp(_currentValue, MinValue, MaxValue);
    }

    public string GetDisplayText()
    {
        if (string.IsNullOrEmpty(_pendingSymbol))
        {
            return FormatValue(_currentValue);
        }

        return $"{FormatValue(_currentValue)} {_pendingSymbol}";
    }

    private string FormatValue(float value)
    {
        return value.ToString("0.#");
    }
}