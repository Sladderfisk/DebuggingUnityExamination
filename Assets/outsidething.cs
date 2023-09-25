using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outsidething : MonoBehaviour
{
        // this is code that another programmer on the project implemented... I don't think it 
        // does anything, but who knows... is it safe to delete??
            
        void Awake() 
    {
        PolygonCollider2D c = GetComponent<PolygonCollider2D>();
        if (c == null) {c = gameObject.AddComponent<PolygonCollider2D>();}
        Vector2[] points = c.points;
        EdgeCollider2D e = gameObject.AddComponent<EdgeCollider2D>();
        e.points = points;
        Destroy(c);  
    }
}
