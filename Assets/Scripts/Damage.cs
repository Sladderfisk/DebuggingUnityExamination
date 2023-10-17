using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Damage : MonoBehaviour
{
    [SerializeField] private bool scaleUpOther;
    [SerializeField] private float maxScaleDelta;
    [SerializeField] private float minScaleDelta;

    private static Dictionary<int, OrganismMovement> organisms;

    private void Start()
    {
        organisms ??= new Dictionary<int, OrganismMovement>();

        CacheOrganisms();
    }

    private static void CacheOrganisms()
    {
        if (organisms.Count != 0) return;

        var organismMovements = FindObjectsOfType<OrganismMovement>();

        foreach (var organism in organismMovements)
        {
            organisms[organism.gameObject.GetInstanceID()] = organism;
        }
    }

    private void OnDestroy()
    {
        organisms.Clear();
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(CompareTag(other, "Player") && scaleUpOther) HandlePlayer(other);
        else if(CompareTag(other, "Organism")) HandleOrganism(other);
    }

    private static bool CompareTag(Collision2D other, string tag)
    {
        return other.gameObject.CompareTag(tag);
    }

    private void HandlePlayer(Collision2D player)
    {
        if(scaleUpOther) GameManager.instance.LoadLoseUI();
    }

    private void HandleOrganism(Collision2D organism)
    {
        var other = organisms[organism.gameObject.GetInstanceID()];
        var scaleDirection = scaleUpOther ? 1 : -1;

        if (other.ScaleDirection == scaleDirection) return;
        other.ScaleDirection = scaleDirection;
        
        var targetDelta = Random.Range(minScaleDelta, maxScaleDelta);
        other.MultiplyScaleBy(new Vector2(targetDelta, targetDelta));
    }
}
