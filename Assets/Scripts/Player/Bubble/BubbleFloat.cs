using UnityEngine;

public class FloatingBubble : MonoBehaviour
{
    float speed = 1f;
     float floatHeight = 0.5f;
     float floatSpeed = 0.5f;
    private float randomSeed;
    [SerializeField] GameObject particlePrefab;
    private Vector3 startPosition;
    [SerializeField] Vector2 floatHeightRangeScale;
    [SerializeField] Vector2 floatSpeedRangeScale;
    
	private float _deltaX;
	private float _maxX;
    void Start()
    {
        startPosition = transform.position;
        randomSeed = Random.value;
        speed = Random.Range(floatSpeedRangeScale.x, floatSpeedRangeScale.y);
        floatHeight = Random.Range(floatHeightRangeScale.x, floatHeightRangeScale.y);

		_maxX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x + 1;
    }

    void Update()
    {
		_deltaX += speed * Time.deltaTime;
        float moveX = startPosition.x + _deltaX;


        float randomFactor = Mathf.PerlinNoise((Time.time + randomSeed) * floatSpeed, 0f);
        float moveY = startPosition.y + Mathf.Lerp(-floatHeight, floatHeight, randomFactor);

        transform.position = new Vector3(moveX, moveY, transform.position.z);

		if(moveX > _maxX)
		{
			Destroy(gameObject);
		}
    }

	private void OnDestroy()
	{
		SpawnParticle();
	}

	void SpawnParticle()
    {
        if (particlePrefab != null)
        {
            // Instantiate the particle prefab at the object's position
            Instantiate(particlePrefab, transform.position, Quaternion.identity);

            // Optionally, destroy the particle system after it finishes
            ParticleSystem ps = particlePrefab.GetComponent<ParticleSystem>();
           
        }
        else
        {
            Debug.LogWarning("Particle prefab not assigned!");
        }
    }
}