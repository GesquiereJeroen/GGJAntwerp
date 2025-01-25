using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int NumberOfBubbles;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform spawnPos;
    //[SerializeField]Vector2 SpawnLocation;
    public static List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < NumberOfBubbles; i++)
        {
            SpawneBubble();
        }
    }

    void SpawneBubble()
    {
       spawnedObjects.Add(Instantiate(objectToSpawn, new Vector2( spawnPos.position.x, spawnPos.position.y) + new Vector2(Random.Range(-1,1),Random.Range(-1,1)), Quaternion.identity)) ;
    }
}

