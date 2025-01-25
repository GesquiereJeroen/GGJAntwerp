using UnityEngine;

public class FloatingBubble : MonoBehaviour
{
    float speed = 1f;
     float floatHeight = 0.5f;
     float floatSpeed = 0.5f;
    private float randomSeed;

    private Vector3 startPosition;
    [SerializeField] Vector2 floatHeightRangeScale;
    [SerializeField] Vector2 floatSpeedRangeScale;
    void Start()
    {
        startPosition = transform.position;
        randomSeed = Random.value;
        Debug.Log(randomSeed);
        speed = Random.Range(floatSpeedRangeScale.x, floatSpeedRangeScale.y);
        floatHeight = Random.Range(floatHeightRangeScale.x, floatHeightRangeScale.y);
    }

    void Update()
    {

        float moveX = startPosition.x + speed * Time.time;


        float randomFactor = Mathf.PerlinNoise((Time.time + randomSeed) * floatSpeed, 0f);
        float moveY = startPosition.y + Mathf.Lerp(-floatHeight, floatHeight, randomFactor);

        transform.position = new Vector3(moveX, moveY, transform.position.z);
    }
}