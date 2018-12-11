using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeSpawnPoints : MonoBehaviour {

    private Transform[] terrainChunks;

    [Header("Spawn Point Data")]
    public float spawnPointHieght = 0;
    [Range(1, 100)] public int dstBetweenPoints = 1;

    [Space]
    public GameObject enemyToBeSpawned;
    public int numberOfEnemiesToBeSpawned = 1;

    IEnumerator Start() {

        yield return new WaitForSeconds(5);
        Debug.Log("Finished Loading");
        terrainChunks = GetComponentsInChildren<Transform>();
        
        for (int i = 1; i < terrainChunks.Length; i++)
        {
             var newSpawnMesh = terrainChunks[i].gameObject.AddComponent<SpawnPointsOnMesh>();
             MeshFilter filter = terrainChunks[i].GetComponent<MeshFilter>();
             newSpawnMesh.spawnPointHieght = spawnPointHieght;
             newSpawnMesh.dstBetweenPoints = dstBetweenPoints;
             newSpawnMesh.enemyToBeSpawned = enemyToBeSpawned;
             newSpawnMesh.numberOfEnemiesToBeSpawned = numberOfEnemiesToBeSpawned;
             newSpawnMesh.spawnGrid = new SpawnPointNode[filter.mesh.vertexCount + 1];
             newSpawnMesh.spawnNodesOnMesh(terrainChunks[i].GetComponent<MeshFilter>().mesh);
            // newSpawnMesh.spawnEnemies(newSpawnMesh.enemyToBeSpawned, newSpawnMesh.numberOfEnemiesToBeSpawned);
        }

    }
}
