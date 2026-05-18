using System.Collections.Generic;
using UnityEngine;

public class EnemyDraftPicker : MonoBehaviour
{
    public int PickAny(int cardCount)
    {
        return Random.Range(0, cardCount);
    }

    public int PickExcept(int cardCount, int blockedIndex)
    {
        List<int> availableIndexes = new();

        for (int i = 0; i < cardCount; i++)
        {
            if (i != blockedIndex)
            {
                availableIndexes.Add(i);
            }
        }

        return availableIndexes[Random.Range(0, availableIndexes.Count)];
    }
}