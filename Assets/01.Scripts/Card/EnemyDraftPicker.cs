using System.Collections.Generic;
using UnityEngine;

public class EnemyDraftPicker : MonoBehaviour
{
    private const float LowBias = 1.25f;
    private const int MaxOperationCount = 4;

    private readonly List<string> _pickedSymbols = new();
    private readonly List<string> _pickedNumbers = new();

    private float _lowValue = 1f;
    private float _highValue = 1f;
    private int _lowOperationCount;
    private int _highOperationCount;

    public float LowValue => _lowValue;
    public float HighValue => _highValue;

    public int PickAny(string[] candidates, DraftType draftType)
    {
        return PickBestIndex(candidates, draftType, -1);
    }

    public int PickExcept(string[] candidates, DraftType draftType, int blockedIndex)
    {
        return PickBestIndex(candidates, draftType, blockedIndex);
    }

    public void RecordPicked(string value, DraftType draftType)
    {
        if (draftType == DraftType.Symbol)
        {
            _pickedSymbols.Add(value);
            return;
        }

        _pickedNumbers.Add(value);
    }
    public void ResetHands()
    {
        _pickedSymbols.Clear();
        _pickedNumbers.Clear();

        _lowValue = 1f;
        _highValue = 1f;
        _lowOperationCount = 0;
        _highOperationCount = 0;
    }

    public void ResolvePickedCards()
    {
        for (int i = 0; i < _pickedSymbols.Count; i++)
        {
            string symbol = _pickedSymbols[i];
            float number = float.Parse(_pickedNumbers[i]);

            float nextLow = Apply(_lowValue, symbol, number);
            float nextHigh = Apply(_highValue, symbol, number);

            float lowScore = ScoreLow(nextLow) * LowBias;
            float highScore = ScoreHigh(nextHigh);

            bool canUseLow = _lowOperationCount < MaxOperationCount;
            bool canUseHigh = _highOperationCount < MaxOperationCount;

            if (!canUseLow && !canUseHigh)
            {
                Debug.Log($"적이 {symbol} {number:0.#}를 사용하지 못함: Low/High 남은 횟수 없음");
                continue;
            }

            if (!canUseLow)
            {
                Debug.Log($"적이 High에 {symbol} {number:0.#} 적용: {_highValue:0.#} -> {nextHigh:0.#}");
                _highValue = nextHigh;
                _highOperationCount++;
                continue;
            }

            if (!canUseHigh)
            {
                Debug.Log($"적이 Low에 {symbol} {number:0.#} 적용: {_lowValue:0.#} -> {nextLow:0.#}");
                _lowValue = nextLow;
                _lowOperationCount++;
                continue;
            }

            if (ShouldUseHighInstead(lowScore, highScore))
            {
                Debug.Log($"적이 High에 {symbol} {number:0.#} 적용: {_highValue:0.#} -> {nextHigh:0.#}");
                _highValue = nextHigh;
                _highOperationCount++;
            }
            else
            {
                Debug.Log($"적이 Low에 {symbol} {number:0.#} 적용: {_lowValue:0.#} -> {nextLow:0.#}");
                _lowValue = nextLow;
                _lowOperationCount++;
            }
        }

        Debug.Log($"적 현재 손 값 / Low: {_lowValue:0.#} 남은 횟수:{MaxOperationCount - _lowOperationCount}, High: {_highValue:0.#} 남은 횟수:{MaxOperationCount - _highOperationCount}");

        _pickedSymbols.Clear();
        _pickedNumbers.Clear();
    }

    private int PickBestIndex(string[] candidates, DraftType draftType, int blockedIndex)
    {
        int bestIndex = -1;
        float bestScore = float.MinValue;

        for (int i = 0; i < candidates.Length; i++)
        {
            if (i == blockedIndex)
            {
                continue;
            }

            float score = EvaluateCandidate(candidates[i], draftType);

            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = i;
            }
        }

        Debug.Log($"적이 {GetDraftName(draftType)} 후보 중 {candidates[bestIndex]} 선택");
        return bestIndex;
    }

    private float EvaluateCandidate(string value, DraftType draftType)
    {
        if (draftType == DraftType.Symbol)
        {
            return EvaluateSymbol(value);
        }

        return EvaluateNumber(float.Parse(value));
    }

    private float EvaluateSymbol(string symbol)
    {
        if (symbol == "\u00D7")
        {
            return 100f;
        }

        if (symbol == "-")
        {
            return 70f * LowBias;
        }

        if (symbol == "+")
        {
            return 55f;
        }

        return 35f;
    }

    private float EvaluateNumber(float number)
    {
        string symbol = _pickedSymbols.Count > _pickedNumbers.Count
            ? _pickedSymbols[_pickedSymbols.Count - 1]
            : "\u00D7";

        float nextLow = Apply(_lowValue, symbol, number);
        float nextHigh = Apply(_highValue, symbol, number);

        float lowScore = ScoreLow(nextLow) * LowBias;
        float highScore = ScoreHigh(nextHigh);

        return Mathf.Max(lowScore, highScore);
    }

    private bool ShouldUseHighInstead(float lowScore, float highScore)
    {
        if (lowScore < 20f && highScore > lowScore + 10f)
        {
            return true;
        }

        return highScore > lowScore + 25f;
    }

    private float ScoreLow(float value)
    {
        return -value;
    }

    private float ScoreHigh(float value)
    {
        return value;
    }

    private float Apply(float currentValue, string symbol, float number)
    {
        float result = currentValue;

        if (symbol == "+")
        {
            result += number;
        }
        else if (symbol == "-")
        {
            result -= number;
        }
        else if (symbol == "\u00D7")
        {
            result *= number;
        }
        else if (symbol == "\u00F7")
        {
            result /= number;
        }

        return Mathf.Clamp(result, -1000f, 1000f);
    }

    private string GetDraftName(DraftType draftType)
    {
        return draftType == DraftType.Symbol ? "기호" : "숫자";
    }
}