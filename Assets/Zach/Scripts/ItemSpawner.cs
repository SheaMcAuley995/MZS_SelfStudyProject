using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    List<GameObject> itemList = new List<GameObject>();
    public GameObject scrap, scrapsParent;

    public Vector3 center, size;
    float radius;
    public Collider[] colliders;
    public LayerMask mask;

    [Tooltip("Don't touch this, it's just visible here so that I can see how many there are in any particular session")]
    [SerializeField] int totalScraps;

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

        GameObject BoosterInjection = Instantiate(itemList[0], spawnPos, Quaternion.identity, scrapsParent.transform);
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
    }

    void SetChancesForItems()
    {
        totalScraps = Random.Range(5, 20);
    }
}
