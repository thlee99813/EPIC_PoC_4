using UnityEngine;
using UnityEngine.EventSystems;

public class HandDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private HandView _handView;
    [SerializeField] private SystemLogView _systemLogView;
    [SerializeField] private PickedCardPanel _pickedCardPanel;



    private readonly HandModel _handModel = new();

    private void Start()
    {
        _handView.SetText(_handModel.GetDisplayText());
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!_pickedCardPanel.CanPlaceCards)
        {
            _systemLogView.ShowNeedAllCardsPicked();
            return;
        }

        DraggablePickedCard card = eventData.pointerDrag.GetComponent<DraggablePickedCard>();

        if (!_handModel.TryAdd(card.Value, card.CardType))
        {
            if (_handModel.NeedsSymbol)
            {
                _systemLogView.ShowNeedSymbol();
                return;
            }

            _systemLogView.ShowNeedNumber();
            return;
        }

        _handView.SetText(_handModel.GetDisplayText());
        _systemLogView.ShowCardPlaced();
        card.gameObject.SetActive(false);
    }
}