using System;
using UnityEngine;

public class DraftFieldView : MonoBehaviour
{
    [SerializeField] private CardView[] _cards;

    public int CardCount => _cards.Length;

    public void ShowFrontCards(string[] candidates, Action<int, string> onClicked)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].SetSelectable(i, candidates[i], onClicked);
        }
    }

    public void ShowBackCards(string[] candidates, Action<int, string> onClicked)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].SetSelectable(i, candidates[i], onClicked);
            _cards[i].ShowBackLocked();
        }
    }

    public void HideCard(int index)
    {
        _cards[index].HideCard();
    }

    public void ShowRemainingBackLocked(int hiddenIndex)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            if (i == hiddenIndex)
            {
                continue;
            }

            _cards[i].ShowBackLocked();
        }
    }

    public void ShowRemainingFrontSelectable(int hiddenIndex)
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            if (i == hiddenIndex)
            {
                continue;
            }

            _cards[i].ShowFrontSelectable();
        }
    }

    public void LockAll()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].SetInteractable(false);
        }
    }
}