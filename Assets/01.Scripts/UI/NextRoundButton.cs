using UnityEngine;

public class NextRoundButton : MonoBehaviour
{
    [SerializeField] private UIController _uiController;
    [SerializeField] private DraftPresenter _draftPresenter;

    public void OnClick()
    {
        _uiController.HideNextRoundButton();
        _draftPresenter.StartNextBattleCycle();
    }
}