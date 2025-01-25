using UnityEngine;
using DG.Tweening;

public class PendulumEffectDOTween : MonoBehaviour
{
    [Header("Pendulum Settings")]
    [Tooltip("Start angle of the swing in degrees.")]
    public float startAngle = 5.0f;

    [Tooltip("End angle of the swing in degrees.")]
    public float endAngle = -20.0f;

    [Tooltip("Duration of one full swing (to and fro).")]
    public float swingDuration = 2.0f;

    [Tooltip("Set to true if you want the motion to start immediately.")]
     bool autoStart = true;

    private Tween pendulumTween;

    void Start()
    {
        if (autoStart)
        {
            StartPendulum();
        }
    }

    /// <summary>
    /// Starts the pendulum motion between custom angles.
    /// </summary>
    public void StartPendulum()
    {
        // Ensure any existing tween is killed
        if (pendulumTween != null && pendulumTween.IsActive())
        {
            pendulumTween.Kill();
        }

        // Create a tween for the pendulum effect between startAngle and endAngle
        pendulumTween = transform.DOLocalRotate(
            new Vector3(0, 0, endAngle), // Target rotation
            swingDuration / 2,          // Half duration for one direction
            RotateMode.LocalAxisAdd     // Rotate in local space
        )
        .From(new Vector3(0, 0, startAngle)) // Starting rotation
        .SetEase(Ease.InOutSine)            // Smooth pendulum-like motion
        .SetLoops(-1, LoopType.Yoyo);       // Infinite back-and-forth motion
    }

    /// <summary>
    /// Stops the pendulum motion.
    /// </summary>
    public void StopPendulum()
    {
        if (pendulumTween != null && pendulumTween.IsActive())
        {
            pendulumTween.Kill();
        }
    }

    void OnDestroy()
    {
        // Clean up the tween when the object is destroyed
        StopPendulum();
    }
}
