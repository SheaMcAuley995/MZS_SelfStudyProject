using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    List<GameObject> itemList = new List<GameObject>();
    public GameObject scrap, enemy, scrapsParent, enemyParent;

    public Vector3 center, size;
    float radius;
    public Collider[] colliders;
    public LayerMask mask;

    [Tooltip("Don't touch this, it's just visible here so that I can see how many there are in any particular session")]
    [SerializeField] int totalScraps;
    [SerializeField] int totalEnemies;

    // Use this for initialization
    void Start()
    {
        radius = size.magnitude;

        AddPrefabsToList(); // Adds the prefabs to the item list
        SetChancesForItems(); // Sets the chances for items to spawn

        // Spawner loops
        #region Spawner Loops
        for (int scrapsNumber = 0; scrapsNumber < totalScraps; scrapsNumber++)
        {
            SpawnScraps();
        }

        for (int enemyNumber = 0; enemyNumber < totalEnemies; enemyNumber++)
        {
            SpawnEnemies();
        }
        #endregion       
    }

    // Spawners
    #region Spawners
    void SpawnScraps()
    {
        Vector3 spawnPos = new Vector3(0f, 0f, 0f);
        bool canSpawnHere = false;
        int safetyNet = 0;

        while (!canSpawnHere)
        {
            spawnPos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
            canSpawnHere = PreventSpawnOverlap(spawnPos);

            if (canSpawnHere)
            {
                break;
            }

            safetyNet++;

            if (safetyNet > 50)
            {
                break;
            }
        }

        GameObject NewItem = Instantiate(itemList[0], spawnPos, Quaternion.identity, scrapsParent.transform);
    }

    void SpawnEnemies()
    {
        Vector3 spawnPos = new Vector3(0f, 0f, 0f);
        bool canSpawnHere = false;
        int safetyNet = 0;

        while (!canSpawnHere)
        {
            spawnPos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
            canSpawnHere = PreventSpawnOverlap(spawnPos);

            if (canSpawnHere)
            {
                break;
            }

            safetyNet++;

            if (safetyNet > 50)
            {
                break;
            }
        }

        GameObject Zombie = Instantiate(itemList[1], spawnPos, Quaternion.identity, enemyParent.transform);
    }
    #endregion

    bool PreventSpawnOverlap(Vector3 pos)
    {
        colliders = Physics.OverlapSphere(center, radius, mask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float height = colliders[i].bounds.extents.y;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;
            float lowerExtent = centerPoint.y - height;
            float upperExtent = centerPoint.y + height;

            if (pos.x >= leftExtent && pos.x <= rightExtent)
            {
                if (pos.y >= lowerExtent && pos.y <= upperExtent)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }

    void AddPrefabsToList()
    {
        itemList.Add(scrap);
        itemList.Add(enemy);
    }

    void SetChancesForItems()
    {
        totalScraps = Random.Range(20, 25); // These values will need to go way up for the actual level, I just can't fit them all on my platform
        totalEnemies = Random.Range(5, 10);
    }
}
