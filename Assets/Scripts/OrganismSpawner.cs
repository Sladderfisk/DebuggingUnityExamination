using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrganismSpawner : MonoBehaviour
{
    [SerializeField] private float timeBeforeNextSpawn;
    [SerializeField] private Vector2 maxSpawnArea;
    
    [SerializeField] private GameObject blueOrganism;
    [SerializeField] private GameObject redOrganism;

    [Space]
    [SerializeField] private int amountToCacheForEach;

    private float timeSinceLastSpawn;
    private Type lastType;

    private List<GameObject> cachedRed;
    private List<GameObject> cachedBlue;

    private List<GameObject> activeRed;
    private List<GameObject> activeBlue;

    private void Awake()
    {
        cachedRed = new List<GameObject>();
        cachedBlue = new List<GameObject>();
        activeRed = new List<GameObject>();
        activeBlue = new List<GameObject>();
        
        Cache();
    }

    private void Cache()
    {
        for (int c = 0; c < amountToCacheForEach; c++)
        {
            var red = Instantiate(redOrganism);
            red.SetActive(false);
            cachedRed.Add(red);
            
            var blue = Instantiate(blueOrganism);
            blue.SetActive(false);
            cachedBlue.Add(blue);
        }
    }

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn > timeBeforeNextSpawn)
        {
            lastType = lastType == Type.Blue ? Type.Red : Type.Blue;
            timeSinceLastSpawn = 0;

            var spawnLocation = new Vector2(
                Random.Range(-maxSpawnArea.x, maxSpawnArea.x),
                Random.Range(-maxSpawnArea.y, maxSpawnArea.y));
            Spawn(lastType, spawnLocation);
        }
    }

    private void Spawn(Type type, Vector3 position)
    {
        switch (type)
        {
            case Type.Red:

                if (cachedRed.Count < 1)
                {
                    Debug.LogWarning("There are no more cached red!!!");
                    return;
                }
                
                var red = cachedRed[0];
                cachedRed.Remove(red);
                activeRed.Add(red);
                
                SpawnObject(red, position);
                
                break;
            
            case Type.Blue:
                
                if (cachedBlue.Count < 1)
                {
                    Debug.LogWarning("There are no more cached blue!!!");
                    return;
                }
                
                var blue = cachedBlue[0];
                cachedBlue.Remove(blue);
                activeBlue.Add(blue);
                
                SpawnObject(blue, position);
                
                break;
        }
    }

    private void SpawnObject(GameObject obj, Vector3 pos)
    {
        obj.SetActive(true);
        obj.transform.position = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(Vector3.zero, maxSpawnArea * 2);
    }

    private enum Type
    {
        Red,
        Blue
    }
}
