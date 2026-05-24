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

    private Vector3 launcherStartPos;
    private float currentPullPercent;
    private Coroutine pullRoutine;
    private Coroutine releaseRoutine;

    private void Start()
    {
        launcherStartPos = launcherVisual.localPosition;

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
        if (pullRoutine != null)
            return;

        if (releaseRoutine != null)
        {
            StopCoroutine(releaseRoutine);
            releaseRoutine = null;
        }

        pullRoutine = StartCoroutine(PullLauncherRoutine());
    }

    public void ReleaseLauncher()
    {
        if (pullRoutine != null)
        {
            StopCoroutine(pullRoutine);
            pullRoutine = null;
        }

        if (releaseRoutine != null)
            StopCoroutine(releaseRoutine);

        releaseRoutine =
            StartCoroutine(ReleaseLauncherRoutine());
    }

    private IEnumerator PullLauncherRoutine()
    {
        while (currentPullPercent < 1f)
        {
            currentPullPercent += Time.deltaTime / pullDuration;
            currentPullPercent = Mathf.Clamp01(currentPullPercent);

            UpdateLauncherVisual();

            yield return null;
        }

        pullRoutine = null;
    }

    private IEnumerator ReleaseLauncherRoutine()
    {
        float launchForce =
            currentPullPercent * maxLaunchForce;

        if (launcherTrigger.CurBall != null) 
        {
            launcherTrigger.CurBall.Rb.AddForce( launcherVisual.forward * launchForce, ForceMode.Impulse);
        }

        while (currentPullPercent > 0f)
        {
            currentPullPercent -= Time.deltaTime / releaseDuration;
            currentPullPercent = Mathf.Clamp01(currentPullPercent);

            UpdateLauncherVisual();

            yield return null;
        }

        currentPullPercent = 0f;

        UpdateLauncherVisual();

        releaseRoutine = null;
    }

    private void UpdateLauncherVisual()
    {
        Vector3 pos = launcherStartPos;
        pos.z = launcherStartPos.z - (currentPullPercent * maxPullDistance);
        launcherVisual.localPosition = pos;
    }
}