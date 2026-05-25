using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int maxBallsCount = 3;
    [SerializeField] private Transform spawnPoint;

    private readonly Queue<Ball> _pool = new Queue<Ball>();

    public int MaxBallsCount => maxBallsCount;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxBallsCount; i++)
        {
            Ball ball = Instantiate(ballPrefab, transform);
            ball.gameObject.SetActive(false);
            _pool.Enqueue(ball);
        }
    }

    public Ball SpawnBall()
    {
        if (_pool.Count <= 0)
        {
            Debug.Log("No balls left in pool");
            return null;
        }

        Ball ball = _pool.Dequeue();

        ball.transform.position = spawnPoint.position;
        ball.transform.rotation = spawnPoint.rotation;

        ball.Rb.linearVelocity = Vector3.zero;
        ball.Rb.angularVelocity = Vector3.zero;

        ball.gameObject.SetActive(true);

        return ball;
    }

    public void ReturnBall(Ball ball)
    {
        if (ball == null) return;

        ball.gameObject.SetActive(false);
        _pool.Enqueue(ball);
    }
}