using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        transform.position = randomPositionOnScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
