using System.Collections;
using UnityEngine;

public class DraftPresenter : MonoBehaviour
{
    [SerializeField] private DraftFieldView _fieldView;
    [SerializeField] private PickedCardPanel _playerPickedCardPanel;
    [SerializeField] private DraftCandidateGenerator _candidateGenerator;
    [SerializeField] private EnemyDraftPicker _enemyDraftPicker;
    [SerializeField] private SystemLogView _systemLogView;
    [SerializeField] private float _enemyPickDelay = 1f;
    
    private DraftType _currentDraftType;

    private const int MaxDraftRound = 3;

    private Coroutine _draftRoutine;
    private int _draftStep;
    private int _draftRound;
    private FirstPicker _currentFirstPicker;

    private void Start()
    {
        ShowPlayerFirstDraft(DraftType.Symbol);
    }

    private void ShowPlayerFirstDraft(DraftType draftType)
    {
        _currentDraftType = draftType;
        _currentFirstPicker = FirstPicker.Player;

        _systemLogView.ShowPlayerPick(draftType);

        string[] candidates = _candidateGenerator.CreateCandidates(draftType);
        _fieldView.ShowFrontCards(candidates, OnPlayerPickedCard);
    }
    private void ShowEnemyFirstDraft(DraftType draftType)
    {
        _currentDraftType = draftType;
        _currentFirstPicker = FirstPicker.Enemy;

        _systemLogView.ShowEnemyThinking(draftType);

        string[] candidates = _candidateGenerator.CreateCandidates(draftType);
        _fieldView.ShowBackCards(candidates, OnPlayerPickedCard);

        _draftRoutine = StartCoroutine(ResolveEnemyFirstDraft());
    }

    private void OnPlayerPickedCard(int index, string value)
    {
        if (_draftRoutine != null)
        {
            return;
        }

        _playerPickedCardPanel.Add(value, ConvertCardType(_currentDraftType));

        if (_currentFirstPicker == FirstPicker.Player)
        {
            _draftRoutine = StartCoroutine(ResolvePlayerFirstDraft(index));
            return;
        }

        ResolvePlayerSecondPick(index);
    }

    private IEnumerator ResolvePlayerFirstDraft(int playerPickedIndex)
    {
        _fieldView.HideCard(playerPickedIndex);
        _fieldView.ShowRemainingBackLocked(playerPickedIndex);

        yield return new WaitForSeconds(_enemyPickDelay);

        int enemyPickedIndex = _enemyDraftPicker.PickExcept(_fieldView.CardCount, playerPickedIndex);
        _fieldView.HideCard(enemyPickedIndex);

        yield return new WaitForSeconds(0.5f);

        _draftRoutine = null;
        ShowNextDraft();
    }

    private IEnumerator ResolveEnemyFirstDraft()
    {
        yield return new WaitForSeconds(_enemyPickDelay);

        int enemyPickedIndex = _enemyDraftPicker.PickAny(_fieldView.CardCount);
        _fieldView.HideCard(enemyPickedIndex);
        _fieldView.ShowRemainingFrontSelectable(enemyPickedIndex);
        _systemLogView.ShowPlayerSecondPick(_currentDraftType);

        _draftRoutine = null;
    }

    private void ResolvePlayerSecondPick(int playerPickedIndex)
    {
        _fieldView.HideCard(playerPickedIndex);
        _fieldView.LockAll();

        ShowNextDraft();
    }

    private void ShowNextDraft()
    {
        _draftStep++;

        if (_draftStep == 1)
        {
            ShowEnemyFirstDraft(DraftType.Number);
            return;
        }

        if (_draftStep == 2)
        {
            ShowEnemyFirstDraft(DraftType.Symbol);
            return;
        }

        if (_draftStep == 3)
        {
            ShowPlayerFirstDraft(DraftType.Number);
            return;
        }

        FinishDraftRound();
    }
    private DraftCardType ConvertCardType(DraftType draftType)
    {
        if (draftType == DraftType.Symbol)
        {
            return DraftCardType.Symbol;
        }

        return DraftCardType.Number;
    }
    private void FinishDraftRound()
    {
        _draftRound++;

        if (_draftRound >= MaxDraftRound)
        {
            Debug.Log("All draft rounds finished");
            _systemLogView.ShowDraftFinished();
            return;
        }

        _draftStep = 0;
        ShowPlayerFirstDraft(DraftType.Symbol);
    }
}