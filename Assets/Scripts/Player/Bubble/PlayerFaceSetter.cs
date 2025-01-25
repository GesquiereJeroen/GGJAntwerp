using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaceSetter : MonoBehaviour
{

    [SerializeField]List<Sprite> playerFaces;
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer spriteRenderer;
 

    void SetFace()
    {
        spriteRenderer.sprite = playerFaces[Random.Range(0,playerFaces.Count-1)];
    }
}
