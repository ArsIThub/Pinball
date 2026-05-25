using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PinballController pinballController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BallPool pool;
    [SerializeField] private UiController uiController;
    [SerializeField] private InputListener inputListener;

    private void Awake()
    {
        Physics.gravity = new Vector3(0f, -10f, -4f);
        Physics.defaultMaxDepenetrationVelocity = 0.5f;

        gameManager.Construct(pool, uiController);
        inputListener.Construct(pinballController);
    }
}
