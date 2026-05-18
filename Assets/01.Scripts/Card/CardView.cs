using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _cardFront;
    [SerializeField] private GameObject _cardBack;
    [SerializeField] private TextMeshProUGUI _text;

    private int _index;
    private string _value;
    private Action<int, string> _onClicked;

    private void Awake()
    {
        _button.onClick.AddListener(HandleClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(HandleClick);
    }

    public void SetSelectable(int index, string value, Action<int, string> onClicked)
    {
        gameObject.SetActive(true);

        _index = index;
        _value = value;
        _onClicked = onClicked;

        _text.text = value;
        _cardFront.SetActive(true);
        _text.gameObject.SetActive(true);
        _cardBack.SetActive(false);

        _button.enabled = true;
        _button.interactable = true;
    }

    public void SetDisplay(string value)
    {
        gameObject.SetActive(true);

        _value = value;
        _onClicked = null;

        _text.text = value;
        _cardFront.SetActive(true);
        _text.gameObject.SetActive(true);
        _cardBack.SetActive(false);

        _button.interactable = true;
        _button.enabled = false;
    }

    public void HideCard()
    {
        gameObject.SetActive(false);
    }

    public void ShowBackLocked()
    {
        gameObject.SetActive(true);

        _cardFront.SetActive(false);
        _text.gameObject.SetActive(false);
        _cardBack.SetActive(true);

        _button.enabled = true;
        _button.interactable = false;
    }

    public void ShowFrontSelectable()
    {
        gameObject.SetActive(true);

        _cardFront.SetActive(true);
        _text.gameObject.SetActive(true);
        _cardBack.SetActive(false);

        _button.enabled = true;
        _button.interactable = true;
    }

    public void SetInteractable(bool interactable)
    {
        _button.enabled = true;
        _button.interactable = interactable;
    }

    private void HandleClick()
    {
        _onClicked?.Invoke(_index, _value);
    }
}