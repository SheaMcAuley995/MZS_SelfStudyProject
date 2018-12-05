using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsOnMesh : MonoBehaviour {

    public float spawnPointHieght = 0;
    [Range(1, 100)] public int dstBetweenPoints = 1;
    public SpawnPointNode[] spawnGrid;
    [HideInInspector]public MeshFilter filter;

    

    [Space]
    public GameObject enemyToBeSpawned;
    public int numberOfEnemiesToBeSpawned = 1;

    public SpawnPointsOnMesh(float _spawnPointHieght, int _dstBetweenPoints, MeshFilter _filter, GameObject _enemyToBeSpawned,int _numOfEnemies)
    {
        spawnPointHieght = _spawnPointHieght;
        dstBetweenPoints = _dstBetweenPoints;
        filter = _filter;
        enemyToBeSpawned = _enemyToBeSpawned;
        numberOfEnemiesToBeSpawned = _numOfEnemies;
    }


    private void Start()
    {  
        filter = GetComponent<MeshFilter>();
        spawnGrid = new SpawnPointNode[filter.mesh.vertexCount + 1];

        var terrainChunks = GetComponents<ChunkManager.TerrainChunk>();

        foreach(ChunkManager.TerrainChunk n in terrainChunks)
        {
            SpawnPointsOnMesh newSpawnMesh = n.meshObj.AddComponent<SpawnPointsOnMesh>();
            newSpawnMesh.spawnPointHieght = spawnPointHieght;
            newSpawnMesh.dstBetweenPoints = dstBetweenPoints;
            newSpawnMesh.enemyToBeSpawned = enemyToBeSpawned;
            newSpawnMesh.numberOfEnemiesToBeSpawned = numberOfEnemiesToBeSpawned;
            newSpawnMesh.spawnNodesOnMesh(n.meshObj.GetComponent<MeshFilter>().mesh);
            newSpawnMesh.spawnEnemies(newSpawnMesh.enemyToBeSpawned, newSpawnMesh.numberOfEnemiesToBeSpawned);
        }
    }

    public void spawnNodesOnMesh(Mesh mesh)
    {
        for(int i = 0; i < mesh.vertexCount; i += dstBetweenPoints)
        {
            Vector3 worldSpacePos = transform.TransformPoint(new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z));
           bool isSpawnable = (worldSpacePos.y > spawnPointHieght);
           spawnGrid[i] = new SpawnPointNode(isSpawnable, worldSpacePos);
           Debug.Log(worldSpacePos);
        }
    }

    public void Initialize()
    {
        spawnNodesOnMesh(filter.mesh);
        spawnEnemies(enemyToBeSpawned, numberOfEnemiesToBeSpawned);
    }

    public void spawnEnemies(GameObject enemy, int enemiesToBeSpawned)
    {
        for(int i = 0; i < enemiesToBeSpawned; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnGrid.Length);
            if(spawnGrid[spawnPointIndex].spawnable)
            {
                Instantiate(enemyToBeSpawned, spawnGrid[spawnPointIndex].worldPosition, transform.rotation);
            }
            else
            {
                i--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        foreach(SpawnPointNode n in spawnGrid)
        {
            if(!n.spawnable)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            
            Gizmos.DrawSphere(n.worldPosition, 0.1f);
        }
    }
}
