using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;
    Vector3[] verts;
    int[] tris;
    Vector2[] uvs;

    public int xSize = 15;
    public int zSize = 15;
    public float waittime = .1f;
    public float frequency = 2f;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

       CreateShape();
        
    }
    private void Update()
    {
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    void CreateShape()
    {
        verts = new Vector3[(xSize + 1) * (zSize + 1)];
      
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= zSize; x++)
            {
                float y = Mathf.PerlinNoise(x*.3f, z*.3f)*3f;
                verts[i] = new Vector3(x, y, z);
                i++;
            }
        }
        tris = new int[xSize*zSize *6];
        int vert = 0;
        int tri = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {


                tris[tri + 0] = vert + 0;
                tris[tri + 1] = vert + xSize + 1;
                tris[tri + 2] = vert + 1;
                tris[tri + 3] = vert + 1;
                tris[tri + 4] = vert + xSize + 1;
                tris[tri + 5] = vert + xSize + 2;
                vert++;
                tri += 6;
                
             
            }
            vert++;
        }

        uvs = new Vector2[verts.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= zSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < verts.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(verts[i], .15f);
        }
    }
}
