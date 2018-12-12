using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPointsOnMesh : MonoBehaviour {

    [Header("Should I generate Nodes?")]
    [SerializeField] bool generateNodes = false;
    [SerializeField] bool showNodesInInspector = false;
    [Space]
    public float spawnPointHieght = 0;
    [Range(1, 10)] public int dstBetweenPoints = 1;
    public SpawnPointNode[] spawnGrid;
    [HideInInspector]public MeshFilter filter;
    
    [Space]
    public GameObject ScrapsPrefab;
    public int numberOfScrapsToBeSpawned = 1;
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

    private void OnValidate()
    {
        if (generateNodes)
        {
            filter = GetComponent<MeshFilter>();
            spawnGrid = new SpawnPointNode[filter.mesh.vertexCount / dstBetweenPoints +1];
            spawnNodesOnMesh(filter.mesh);
            generateNodes = false;
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void spawnNodesOnMesh(Mesh mesh)
    {
        //int x = 0;
        for(int i = 0; i < filter.mesh.vertexCount; i += dstBetweenPoints)
        {
           Vector3 worldSpacePos = transform.TransformPoint(new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, mesh.vertices[i].z));
           bool isSpawnable = (worldSpacePos.y > spawnPointHieght);
           spawnGrid[i / dstBetweenPoints] = new SpawnPointNode(isSpawnable, worldSpacePos);
        }
        //Debug.Log(mesh.vertexCount);
    }

    public void Initialize()
    {
        spawnObjects(enemyToBeSpawned, numberOfEnemiesToBeSpawned);
        spawnObjects(ScrapsPrefab, numberOfScrapsToBeSpawned);
    }

    public void spawnObjects(GameObject Object, int enemiesToBeSpawned)
    {
        for(int i = 0; i < enemiesToBeSpawned; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnGrid.Length);
            if(spawnGrid[spawnPointIndex].spawnable)
            {
                Instantiate(Object, spawnGrid[spawnPointIndex].worldPosition, transform.rotation);
                spawnGrid[spawnPointIndex].spawnable = false;
            }
            else
            {
                i--;
            }
        }
    }

   private void OnDrawGizmos()
   {
        if(showNodesInInspector)
        {
            foreach (SpawnPointNode n in spawnGrid)
            {
                if (!n.spawnable)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawSphere(n.worldPosition, 0.6f);
            }
        }
   }
}
