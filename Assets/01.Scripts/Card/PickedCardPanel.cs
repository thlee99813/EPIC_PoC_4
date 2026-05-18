using System.Collections.Generic;
using UnityEngine;

public class PickedCardPanel : MonoBehaviour
{
    [SerializeField] private CardView[] _cards;

    private readonly List<string> _pickedValues = new();
    private int _cardIndex;

    public IReadOnlyList<string> PickedValues => _pickedValues;
    public bool CanPlaceCards => _cardIndex >= _cards.Length;
    public void Add(string value, DraftCardType cardType)
    {
        _pickedValues.Add(value);

        CardView card = _cards[_cardIndex];
        card.SetDisplay(value);

        DraggablePickedCard draggableCard = card.GetComponent<DraggablePickedCard>();
        draggableCard.SetData(value, cardType);

        _cardIndex++;
    }
}