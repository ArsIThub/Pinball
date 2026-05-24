using UnityEngine;

public class LauncherTrigger : MonoBehaviour
{
    private Ball _curBall;

    public Ball CurBall => _curBall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))
            _curBall = ball;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ball ball))
            _curBall = null;
    }
}
