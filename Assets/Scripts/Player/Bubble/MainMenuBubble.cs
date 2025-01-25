using UnityEngine;
using DG.Tweening;

public class MainMenuBubble : MonoBehaviour
{
    [Header("Movement Settings")]
    public float floatRange = 1f; // Maximum distance the bubble can float in any direction
    public float duration = 2f;  // Time it takes to complete one movement cycle
    public Vector3 basePosition; // Starting position of the bubble

    [Header("Scale Settings")]
    public float scaleRange = 0.2f; // Range for subtle size changes
    public float scaleDuration = 1.5f; // Time it takes to complete one scaling cycle

    private void Start()
    {
        basePosition = transform.position; // Set the starting position
        StartFloating();
        StartScaling();
    }

    private void StartFloating()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-floatRange, floatRange),
            Random.Range(-floatRange, floatRange),
            Random.Range(-floatRange, floatRange)
        );

        Vector3 targetPosition = basePosition + randomOffset;

        // Smoothly move to the target position and loop
        transform.DOMove(targetPosition, duration)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(StartFloating); // Loop movement
    }

    private void StartScaling()
    {
        float randomScale = 0.5f + Random.Range(-scaleRange, scaleRange);

        // Smoothly change scale and loop
        transform.DOScale(randomScale, scaleDuration)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(StartScaling); // Loop scaling
    }
}
