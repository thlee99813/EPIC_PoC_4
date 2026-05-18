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
        List<int> numbers = new();

        for (int i = 1; i <= 9; i++)
        {
            numbers.Add(i);
        }

        Shuffle(numbers);

        return new[]
        {
            numbers[0].ToString(),
            numbers[1].ToString(),
            numbers[2].ToString()
        };
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