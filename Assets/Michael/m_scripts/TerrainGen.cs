using System;
using UnityEngine;

public class TerrainGen : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public int depth = 20;
    public float scale = 20;

    void Start()
    {
        Terrain terr = GetComponent<Terrain>();
        terr.terrainData = GenerateTerrain(terr.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalcHeight(x, y);
            }
        }
        return heights;
    }

    private float CalcHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
