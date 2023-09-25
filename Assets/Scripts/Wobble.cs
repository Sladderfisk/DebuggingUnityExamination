using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // should probably make it so the things cant look bad
        transform.localScale = transform.localScale + new Vector3(Random.Range(-.002f,.002f), Random.Range(-.002f,.002f), 0);
    }
}
