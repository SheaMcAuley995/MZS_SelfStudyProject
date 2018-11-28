using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen
{
    public static MeshData GennerateTerrainMesh(float[,] heightMap, float heightMul, AnimationCurve curve, int lod)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int lodIncrement = (lod == 0)?1: lod * 2;
        int vertPerLine = (width - 1) / lodIncrement + 1;

        MeshData meshData = new MeshData(vertPerLine, vertPerLine);

        int vertIdx = 0;

        for (int y = 0; y < height; y+=lodIncrement)
        {
            for (int x = 0; x < width; x+=lodIncrement)
            {
                meshData.verts[vertIdx] = new Vector3(topLeftX+ x, curve.Evaluate(heightMap[x,y])*heightMul,topLeftZ - y);
                meshData.uvs[vertIdx] = new Vector2(x/(float)width,y/ (float)height);
                if (x < width -1 && y < height -1)
                {
                    meshData.AddTris(vertIdx, vertIdx + vertPerLine + 1, vertIdx + vertPerLine);
                    meshData.AddTris(vertIdx + vertPerLine + 1, vertIdx , vertIdx + 1);
                }
                vertIdx++;
            }
        }
        return meshData;
    }
	
}


public class MeshData
{

    public Vector3[] verts;
    public int[] tris;
    public Vector2[] uvs;
    int triIdx;

    public MeshData(int meshWidth, int meshHeight)
    {
        verts = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        tris = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }
    public void AddTris(int a, int b, int c)
    {
        tris[triIdx] = a;
        tris[triIdx + 1] = b;
        tris[triIdx + 2] = c;
        triIdx += 3;
    }
    public Mesh CreateMessh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}

