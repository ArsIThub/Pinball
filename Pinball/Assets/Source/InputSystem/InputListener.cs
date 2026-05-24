using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviour
{
    private MainInputActions _mainInputActions;
    private PinballController _pinballController;

    public void Construct(PinballController pinballController) 
    {
        _pinballController = pinballController;
    }

    private void Awake()
    {
        _mainInputActions = new();
        Bind();
        _mainInputActions.Enable();
    }

    private void OnLeftFlipperPress(InputAction.CallbackContext context) 
    {
        _pinballController.PressLeftFlipper();
    }
    private void OnLeftFlipperRelease(InputAction.CallbackContext context)
    {
        _pinballController.ReleaseLeftFlipper();
    }

    private void OnRightFlipperPress(InputAction.CallbackContext context) 
    {
        _pinballController.PressRightFlipper();
    }
    private void OnRightFlipperRelease(InputAction.CallbackContext context)
    {
        _pinballController.ReleaseRightFlipper();
    }

    private void OnSpringPull(InputAction.CallbackContext context)
    {
        _pinballController.PullLauncher();
    }
    private void OnSpringRelease(InputAction.CallbackContext context)
    {
        _pinballController.ReleaseLauncher();
    }

    private void Bind() 
    {
        _mainInputActions.Game.LeftFlipper.performed += OnLeftFlipperPress;
        _mainInputActions.Game.LeftFlipper.canceled += OnLeftFlipperRelease;

        _mainInputActions.Game.RightFlipper.performed += OnRightFlipperPress;
        _mainInputActions.Game.RightFlipper.canceled += OnRightFlipperRelease;

        _mainInputActions.Game.Spring.performed += OnSpringPull;
        _mainInputActions.Game.Spring.canceled += OnSpringRelease;
    }
    private void Expose() 
    {
        _mainInputActions.Game.LeftFlipper.performed -= OnLeftFlipperPress;
        _mainInputActions.Game.LeftFlipper.canceled -= OnLeftFlipperRelease;

        _mainInputActions.Game.RightFlipper.performed -= OnRightFlipperPress;
        _mainInputActions.Game.RightFlipper.canceled -= OnRightFlipperRelease;

        _mainInputActions.Game.Spring.performed += OnSpringPull;
        _mainInputActions.Game.Spring.canceled -= OnSpringRelease;
    }

    private void OnDestroy()
    {
        Expose();
        _mainInputActions.Disable();
    }
}
