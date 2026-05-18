using UnityEngine;
using UnityEngine.EventSystems;

public class HoverImageToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _targetImage;

    private void Awake()
    {
        _targetImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetImage.SetActive(false);
    }
}