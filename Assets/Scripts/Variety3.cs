using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variety3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "dontforgettoupdatethetag")
        {
            print("got me");
            Destroy(other.gameObject);
            transform.localScale += new Vector3 (.5f,.5f,0);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localScale += new Vector3 (Random.Range(-.02f,.02f), Random.Range(-.02f,.02f), Random.Range(0,0));
        transform.position = transform.position + new Vector3(Random.Range(-.01f,.01f), Random.Range(-.01f,.01f), Random.Range(0,0));
    }
}
