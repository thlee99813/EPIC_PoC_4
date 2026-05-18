using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _nextRoundButton;
    [SerializeField] private GameObject _gameOverImage;
    [SerializeField] private GameObject _gameClearImage;

    private void Start()
    {
        HideNextRoundButton();
        _gameOverImage.SetActive(false);
        _gameClearImage.SetActive(false);
    }

    public void ShowNextRoundButton()
    {
        _nextRoundButton.SetActive(true);
    }

    public void HideNextRoundButton()
    {
        _nextRoundButton.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverImage.SetActive(true);
    }

    public void ShowGameClear()
    {
        _gameClearImage.SetActive(true);
    }
}