using UnityEngine;

public class NextRoundButton : MonoBehaviour
{
    [SerializeField] private GameObject _buttonRoot;
    [SerializeField] private DraftPresenter _draftPresenter;

    public void OnClick()
    {
        _buttonRoot.SetActive(false);
        _draftPresenter.StartNextBattleCycle();
    }
}