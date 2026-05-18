using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePickedCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private PickedCardPanel _pickedCardPanel;
    [SerializeField] private SystemLogView _systemLogView;

    private Vector2 _startAnchoredPosition;
    private bool _isDragging;

    public string Value { get; private set; }
    public DraftCardType CardType { get; private set; }

    public void SetData(string value, DraftCardType cardType)
    {
        Value = value;
        CardType = cardType;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_pickedCardPanel.CanPlaceCards)
        {
            _systemLogView.ShowNeedAllCardsPicked();
            _isDragging = false;
            return;
        }

        _isDragging = true;
        _startAnchoredPosition = _rectTransform.anchoredPosition;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            return;
        }

        _rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            return;
        }

        _isDragging = false;
        _canvasGroup.blocksRaycasts = true;
        _rectTransform.anchoredPosition = _startAnchoredPosition;
    }
}