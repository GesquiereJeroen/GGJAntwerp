using DG.Tweening;
using UnityEngine;

public class PlayerHeadShake : MonoBehaviour
{
	public float shakeRadius = 0.2f; // Radius of the jitter.
	public float duration = 0.1f;    // Time for each jitter cycle.
	private Tween jitterTween;

	void Start()
	{
		StartJitteryShake();

		GameManager.Instance.GameEnded += EndGameShake;
	}

	private void EndGameShake(object sender, bool e)
	{
		StopJitteryShake();
		if (!e)
		{
			// Stop any existing shake
			StopJitteryShake();

			// Set up a continuous jitter effect
			jitterTween = transform.DOShakeRotation(duration, shakeRadius, vibrato: 20, randomness: 100, fadeOut: false)
			.SetLoops(-1, LoopType.Restart); // Infinite loop
		}
	}

	public void StartJitteryShake()
	{
		// Stop any existing shake
		StopJitteryShake();

		// Set up a continuous jitter effect
		jitterTween = transform.DOShakeRotation(duration, shakeRadius, vibrato: 20, randomness: 100, fadeOut: false)
			.SetLoops(-1, LoopType.Restart); // Infinite loop
	}

	public void StopJitteryShake()
	{
		if (jitterTween != null)
		{
			jitterTween.Kill(); // Stop the shake
			jitterTween = null;
			//transform.localPosition = Vector3.zero; // Reset position
		}
	}
}
