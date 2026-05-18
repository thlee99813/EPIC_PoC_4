using UnityEngine;
using UnityEngine.EventSystems;

public class HandDropZone : MonoBehaviour, IDropHandler
{
    [SerializeField] private HandView _handView;
    [SerializeField] private SystemLogView _systemLogView;
    [SerializeField] private PickedCardPanel _pickedCardPanel;



    private readonly HandModel _handModel = new();
    public float CurrentValue => _handModel.CurrentValue;

    private void Start()
    {
        RefreshView();
    }
    public void ResetHand()
    {
        _handModel.Reset();
        RefreshView();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!_pickedCardPanel.CanPlaceCards)
        {
            _systemLogView.ShowNeedAllCardsPicked();
            return;
        }

        DraggablePickedCard card = eventData.pointerDrag.GetComponent<DraggablePickedCard>();
        if (card.CardType == DraftCardType.Symbol && !_handModel.CanStartOperation)
        {
            _systemLogView.ShowNoRemainingOperation();
            return;
        }
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

        RefreshView();
        _systemLogView.ShowCardPlaced();

        card.ResetDragState();
        card.gameObject.SetActive(false);

        _pickedCardPanel.MarkPlaced();
    }
    private void RefreshView()
    {
        _handView.SetText(_handModel.GetDisplayText());
        _handView.SetRemainingCount(_handModel.RemainingOperationCount);
    }
}