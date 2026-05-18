using TMPro;
using UnityEngine;

public class SystemLogView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    public void ShowPlayerPick(DraftType draftType)
    {
        _logText.text = $"{GetDraftName(draftType)} 카드를 선택하세요.";
    }

    public void ShowEnemyThinking(DraftType draftType)
    {
        _logText.text = $"적이 어떤 {GetDraftName(draftType)}를 뽑을지 고민중입니다.";
    }

    public void ShowPlayerSecondPick(DraftType draftType)
    {
        _logText.text = $"남은 {GetDraftName(draftType)} 카드 중 하나를 선택하세요.";
    }

    public void ShowDraftFinished()
    {
        _logText.text = "드래프트가 끝났습니다. 왼손 또는 오른손에 카드를 배치하세요.";
    }
    public void ShowNeedAllCardsPicked()
    {
        _logText.text = "카드를 먼저 다 뽑아야합니다.";
    }

    public void ShowNeedSymbol()
    {
        _logText.text = "먼저 기호를 넣어야합니다.";
    }

    public void ShowNeedNumber()
    {
        _logText.text = "이제 숫자를 넣어야합니다.";
    }
    public void ShowNoRemainingOperation()
    {
        _logText.text = "이 손에는 더 이상 배치할 수 없습니다.";
    }

    public void ShowCardPlaced()
    {
        _logText.text = "카드를 배치했습니다.";
    }
    public void ShowRaw(string message)
    {
        _logText.text = message;
    }

    private string GetDraftName(DraftType draftType)
    {
        if (draftType == DraftType.Symbol)
        {
            return "기호";
        }

        return "숫자";
    }
}