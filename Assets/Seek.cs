using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // CHALLENGE: This could be more efficient
        GameObject moveTowardsThis = GameObject.FindWithTag("Player");
        transform.position = Vector3.MoveTowards(transform.position, moveTowardsThis.transform.position, 0.005f);
        // put in player's position
    }
}
