using System.Collections.Generic;
using UnityEngine;

public class DraftCandidateGenerator : MonoBehaviour
{
    private readonly string[] _symbols =
    {
        "+",
        "-",
        "\u00D7",
        "\u00F7"
    };
    private readonly List<int> _numberPool = new();
    public void ResetNumberPool()
    {
        _numberPool.Clear();

        for (int i = 1; i <= 9; i++)
        {
            _numberPool.Add(i);
        }
    }
    public string[] CreateCandidates(DraftType draftType)
    {
        if (draftType == DraftType.Symbol)
        {
            return PickRandomSymbols();
        }

        return PickRandomNumbers();
    }

    private string[] PickRandomSymbols()
    {
        string[] candidates = (string[])_symbols.Clone();
        Shuffle(candidates);

        return new[]
        {
            candidates[0],
            candidates[1],
            candidates[2]
        };
    }

    private string[] PickRandomNumbers()
    {
        Shuffle(_numberPool);

        string[] candidates =
        {
            _numberPool[0].ToString(),
            _numberPool[1].ToString(),
            _numberPool[2].ToString()
        };

        _numberPool.RemoveRange(0, 3);

        return candidates;
    }

    private void Shuffle<T>(IList<T> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            int randomIndex = Random.Range(i, values.Count);
            (values[i], values[randomIndex]) = (values[randomIndex], values[i]);
        }
    }
}