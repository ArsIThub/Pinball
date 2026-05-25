using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text ballsLeftText;
    [Space]
    [SerializeField] private GameOverPanel gameOverPanel;

    public void UpdateScoreText(int score) 
    {
        scoreText.text = $"{score}";
    }

    public void UpdateBallsLeftText(int ballsLeftCount)
    {
        ballsLeftText.text = $"Balls left: {ballsLeftCount}";
    }

    public void GameOver(int score) 
    {
        gameOverPanel.ActivatePanel(score);
    }
}
