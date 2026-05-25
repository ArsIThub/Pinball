using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int scorePerCollision = 50;

    private int _curScore = 0;
    private int _ballsLeftCount;
    private BallPool _pool;
    private UiController _uiController;

    public void Construct(BallPool pool, UiController uiController) 
    {
        _pool = pool;
        _uiController = uiController;

        _ballsLeftCount = _pool.MaxBallsCount;
    }

    private void Start()
    {
        _uiController.UpdateScoreText(_curScore);
        _uiController.UpdateBallsLeftText(_ballsLeftCount);

        SpawnBall();
    }

    private void AddScore() 
    {
        _curScore += scorePerCollision;
        _uiController.UpdateScoreText(_curScore);
    }

    private void BallLeaveGameField() 
    {
        if (_ballsLeftCount > 0)
            SpawnBall();

        else EndGame();
    }

    private void SpawnBall() 
    {
        Ball spawnedBall = _pool.SpawnBall();
        spawnedBall.Construct(_pool);
        spawnedBall.OnInteractiveObjectCollision += AddScore;
        spawnedBall.OnLeaveGameField += BallLeaveGameField;

        _ballsLeftCount--;
        _uiController.UpdateBallsLeftText(_ballsLeftCount);
    }

    private void EndGame() 
    {
        _uiController.GameOver(_curScore);
    }
}
