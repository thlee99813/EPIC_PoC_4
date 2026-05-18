using System.Collections.Generic;
using UnityEngine;

public class PickedCardPanel : MonoBehaviour
{
    [SerializeField] private CardView[] _cards;

    private readonly List<string> _pickedValues = new();
    private int _cardIndex;
    private int _placedCardCount;
    private bool _canPlaceCards;
    private System.Action _onAllCardsPlaced;

    public IReadOnlyList<string> PickedValues => _pickedValues;
    public bool CanPlaceCards => _canPlaceCards;
    public void Add(string value, DraftCardType cardType)
    {
        _pickedValues.Add(value);

        CardView card = _cards[_cardIndex];
        card.SetDisplay(value);

        DraggablePickedCard draggableCard = card.GetComponent<DraggablePickedCard>();
        draggableCard.SetData(value, cardType);

        _cardIndex++;
    }

    public void AddHidden(string value, DraftCardType cardType)
    {
        _pickedValues.Add(value);

        CardView card = _cards[_cardIndex];
        card.SetDisplay(value);
        card.ShowBackLocked();

        _cardIndex++;
    }
    public void StartPlacement(System.Action onAllCardsPlaced)
    {
        _canPlaceCards = true;
        _placedCardCount = 0;
        _onAllCardsPlaced = onAllCardsPlaced;
    }

    public void MarkPlaced()
    {
        _placedCardCount++;

        if (_placedCardCount < _cardIndex)
        {
            return;
        }

        _canPlaceCards = false;
        _onAllCardsPlaced?.Invoke();
    }

    public void Clear()
    {
        _pickedValues.Clear();
        _cardIndex = 0;
        _placedCardCount = 0;
        _canPlaceCards = false;
        _onAllCardsPlaced = null;

        for (int i = 0; i < _cards.Length; i++)
        {
            _cards[i].HideCard();
        }
    }
}