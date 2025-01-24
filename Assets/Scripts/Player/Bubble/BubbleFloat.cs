using UnityEngine;

public class FloatingBubble : MonoBehaviour
{
    public float speed = 1f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;
    private float randomSeed;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        randomSeed = Random.value;
        Debug.Log(randomSeed);
    }

    void Update()
    {

        float moveX = startPosition.x + speed * Time.time;


        float randomFactor = Mathf.PerlinNoise((Time.time + randomSeed) * floatSpeed, 0f);
        float moveY = startPosition.y + Mathf.Lerp(-floatHeight, floatHeight, randomFactor);

        transform.position = new Vector3(moveX, moveY, transform.position.z);
    }
}