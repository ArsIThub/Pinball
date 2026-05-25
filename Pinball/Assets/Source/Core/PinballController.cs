using System.Collections;
using UnityEngine;

public class PinballController : MonoBehaviour
{
    [Header("Flippers")]
    [SerializeField] private HingeJoint leftFlipper;
    [SerializeField] private HingeJoint rightFlipper;

    [Header("Flipper Settings")]
    [SerializeField] private float flipperAngle = 45f;
    [SerializeField] private float flipperSpringForce = 5000f;
    [SerializeField] private float flipperDamper = 150f;

    [Header("Launcher")]
    [SerializeField] private Transform launcherVisual;
    [SerializeField] private LauncherTrigger launcherTrigger;
    [Space]
    [SerializeField] private float maxPullDistance = 0.3f;
    [SerializeField] private float pullDuration = 0.5f;
    [SerializeField] private float releaseDuration = 0.08f;
    [SerializeField] private float maxLaunchForce = 30f;

    private Vector3 _launcherStartPos;
    private float _currentPullPercent;
    private Coroutine _pullRoutine;
    private Coroutine _releaseRoutine;

    private void Start()
    {
        _launcherStartPos = launcherVisual.localPosition;

        SetupFlipper(leftFlipper);
        SetupFlipper(rightFlipper);
    }

    private void SetupFlipper(HingeJoint hinge)
    {
        JointSpring spring = hinge.spring;

        spring.spring = flipperSpringForce;
        spring.damper = flipperDamper;
        spring.targetPosition = 0f;

        hinge.spring = spring;
        hinge.useSpring = true;
    }

    public void PressLeftFlipper()
    {
        SetFlipperAngle(leftFlipper, -flipperAngle);
    }

    public void ReleaseLeftFlipper()
    {
        SetFlipperAngle(leftFlipper, 0f);
    }

    public void PressRightFlipper()
    {
        SetFlipperAngle(rightFlipper, flipperAngle);
    }

    public void ReleaseRightFlipper()
    {
        SetFlipperAngle(rightFlipper, 0f);
    }

    private void SetFlipperAngle(HingeJoint hinge, float angle)
    {
        JointSpring spring = hinge.spring;
        spring.targetPosition = angle;
        hinge.spring = spring;
    }

    public void PullLauncher()
    {
        if (_pullRoutine != null) return;

        if (_releaseRoutine != null)
        {
            StopCoroutine(_releaseRoutine);
            _releaseRoutine = null;
        }

        _pullRoutine = StartCoroutine(PullLauncherRoutine());
    }

    public void ReleaseLauncher()
    {
        if (_pullRoutine != null)
        {
            StopCoroutine(_pullRoutine);
            _pullRoutine = null;
        }

        if (_releaseRoutine != null)
            StopCoroutine(_releaseRoutine);

        _releaseRoutine = StartCoroutine(ReleaseLauncherRoutine());
    }

    private IEnumerator PullLauncherRoutine()
    {
        while (_currentPullPercent < 1f)
        {
            _currentPullPercent += Time.deltaTime / pullDuration;
            _currentPullPercent = Mathf.Clamp01(_currentPullPercent);

            UpdateLauncherVisual();

            yield return null;
        }

        _pullRoutine = null;
    }

    private IEnumerator ReleaseLauncherRoutine()
    {
        float launchForce = _currentPullPercent * maxLaunchForce;

        if (launcherTrigger.CurBall != null) 
        {
            launcherTrigger.CurBall.Rb.AddForce( launcherVisual.forward * launchForce, ForceMode.Impulse);
        }

        while (_currentPullPercent > 0f)
        {
            _currentPullPercent -= Time.deltaTime / releaseDuration;
            _currentPullPercent = Mathf.Clamp01(_currentPullPercent);

            UpdateLauncherVisual();

            yield return null;
        }

        _currentPullPercent = 0f;

        UpdateLauncherVisual();

        _releaseRoutine = null;
    }

    private void UpdateLauncherVisual()
    {
        Vector3 pos = _launcherStartPos;
        pos.z = _launcherStartPos.z - (_currentPullPercent * maxPullDistance);
        launcherVisual.localPosition = pos;
    }
}