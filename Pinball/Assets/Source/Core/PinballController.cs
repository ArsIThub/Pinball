using System.Collections;
using UnityEngine;

public class PinballController : MonoBehaviour
{
    [Header("Flippers")]
    [SerializeField] private GameObject leftFlipper;
    [SerializeField] private GameObject rightFlipper;
    [Space]
    [SerializeField] private float flipperAngle = 45f;
    [SerializeField] private float flipperRotateTime = 0.2f;

    private Quaternion _leftInitialRotation;
    private Quaternion _rightInitialRotation;
    private Coroutine _leftFlipperRoutine;
    private Coroutine _rightFlipperRoutine;

    [Header("Spring")]
    [SerializeField] private GameObject spring;
    [Space]
    [SerializeField] private float springPullDistance = 1f;
    [SerializeField] private float springPullDuration = 1f;
    [SerializeField] private float springReleaseDuration = 0.15f;
    [SerializeField] private float maxSpringForce = 15f;

    private Vector3 _springInitialLocalPos;
    private float _currentPullPercent;
    private float _currentSpringForce;
    private Coroutine _pullRoutine;
    private Coroutine _releaseRoutine;

    private void Start()
    {
        _leftInitialRotation = leftFlipper.transform.localRotation;
        _rightInitialRotation = rightFlipper.transform.localRotation;

        _springInitialLocalPos = spring.transform.localPosition;
    }

    public void PressLeftFlipper()
    {
        if (_leftFlipperRoutine != null)
            StopCoroutine(_leftFlipperRoutine);

        Quaternion targetRotation = _leftInitialRotation * Quaternion.Euler(0f, -flipperAngle, 0f);

        _leftFlipperRoutine = StartCoroutine(RotateFlipper(leftFlipper.transform, targetRotation, flipperRotateTime));
    }

    public void ReleaseLeftFlipper()
    {
        if (_leftFlipperRoutine != null)
            StopCoroutine(_leftFlipperRoutine);

        _leftFlipperRoutine = StartCoroutine(RotateFlipper(leftFlipper.transform, _leftInitialRotation, flipperRotateTime));
    }

    public void PressRightFlipper()
    {
        if (_rightFlipperRoutine != null)
            StopCoroutine(_rightFlipperRoutine);

        Quaternion targetRotation = _rightInitialRotation * Quaternion.Euler(0f, flipperAngle, 0f);

        _rightFlipperRoutine = StartCoroutine(RotateFlipper(rightFlipper.transform, targetRotation, flipperRotateTime));
    }

    public void ReleaseRightFlipper()
    {
        if (_rightFlipperRoutine != null)
            StopCoroutine(_rightFlipperRoutine);

        _rightFlipperRoutine = StartCoroutine(RotateFlipper(rightFlipper.transform, _rightInitialRotation, flipperRotateTime));
    }

    private IEnumerator RotateFlipper(Transform target, Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = target.localRotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        target.localRotation = targetRotation;
    }

    public void PullSpring()
    {
        if (_releaseRoutine != null)
        {
            StopCoroutine(_releaseRoutine);
            _releaseRoutine = null;
        }

        if (_pullRoutine != null)
            return;

        _pullRoutine = StartCoroutine(PullSpringRoutine());
    }

    private IEnumerator PullSpringRoutine()
    {
        while (_currentPullPercent < 1f)
        {
            _currentPullPercent += Time.deltaTime / springPullDuration;
            _currentPullPercent = Mathf.Clamp01(_currentPullPercent);

            UpdateSpringPosition();

            _currentSpringForce = _currentPullPercent * maxSpringForce;

            yield return null;
        }

        _pullRoutine = null;
    }

    public void ReleaseSpring()
    {
        if (_pullRoutine != null)
        {
            StopCoroutine(_pullRoutine);
            _pullRoutine = null;
        }

        if (_releaseRoutine != null)
            StopCoroutine(_releaseRoutine);

        _releaseRoutine = StartCoroutine(ReleaseSpringRoutine());
    }

    private IEnumerator ReleaseSpringRoutine()
    {
        while (_currentPullPercent > 0f)
        {
            _currentPullPercent -= Time.deltaTime / springReleaseDuration;
            _currentPullPercent = Mathf.Clamp01(_currentPullPercent);

            UpdateSpringPosition();

            yield return null;
        }

        _currentPullPercent = 0f;

        UpdateSpringPosition();

        Debug.Log("Spring released with force: " + _currentSpringForce);

        /*
         Тут толкаешь шарик:

         ballRb.AddForce(
             transform.forward * currentSpringForce,
             ForceMode.Impulse
         );
        */

        _currentSpringForce = 0f;
        _releaseRoutine = null;
    }

    private void UpdateSpringPosition()
    {
        Vector3 pos = _springInitialLocalPos;
        pos.z = _springInitialLocalPos.z - (_currentPullPercent * springPullDistance);
        spring.transform.localPosition = pos;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}