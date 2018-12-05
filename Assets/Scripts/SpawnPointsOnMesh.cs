using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsOnMesh : MonoBehaviour {

    [SerializeField] float spawnPointHieght;
    public SpawnPointNode[] spawnGrid;
    public MeshFilter filter;
    

    private void Start()
    {  
        filter = GetComponent<MeshFilter>();
        spawnGrid = new SpawnPointNode[filter.mesh.vertexCount + 1];
        spawnNodesOnMesh(filter.mesh);
    }


    public void spawnNodesOnMesh(Mesh mesh)
    {
        for(int i = 0; i < mesh.vertexCount; i++)
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
