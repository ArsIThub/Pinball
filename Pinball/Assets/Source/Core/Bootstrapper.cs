using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private PinballController pinballController;
    [Space]
    [SerializeField] private InputListener inputListener;

    private void Awake()
    {
        inputListener.Construct(pinballController);
    }
}
