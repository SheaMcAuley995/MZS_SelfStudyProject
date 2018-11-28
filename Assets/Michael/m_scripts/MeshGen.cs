using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen
{
    public static void GennerateTerrainMesh(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        MeshData meshData = new MeshData(width, height);

        int vertIdx = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.verts[vertIdx] = new Vector3(x, heightMap[x, y], y);

                vertIdx++;
            }
        }
    }
	
}


public class MeshData
{

    public Vector3[] verts;
    public int[] tris;

    int triIdx;

    public MeshData(int meshWidth, int meshHeight)
    {
        verts = new Vector3[meshWidth * meshHeight];
        tris = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }
    public void AddTris(int a, int b, int c)
    {
        tris[triIdx] = a;
        tris[triIdx + 1] = b;
        tris[triIdx + 2] = c;
        triIdx += 3;
    }
}

