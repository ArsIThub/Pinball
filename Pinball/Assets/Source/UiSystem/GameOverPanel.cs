using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(Restart);
    }

    public void ActivatePanel(int score) 
    {
        scoreText.text = $"Your score: {score}";
        gameObject.SetActive(true);
    }

    private void Restart() 
    {
        Level.ReloadLevel();
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveAllListeners();
    }
}
