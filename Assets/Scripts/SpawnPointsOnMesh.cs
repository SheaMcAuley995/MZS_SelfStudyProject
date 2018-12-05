using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsOnMesh : MonoBehaviour {

    [SerializeField] float spawnPointHieght;
    [SerializeField] [Range(1, 100)] int dstBetweenPoints = 1;
    public SpawnPointNode[] spawnGrid;
    public MeshFilter filter;
    

    private void Start()
    {  
        filter = GetComponent<MeshFilter>();
        spawnGrid = new SpawnPointNode[filter.mesh.vertexCount + 1];
        
    }

    private void Update()
    {
        spawnNodesOnMesh(filter.mesh);
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
