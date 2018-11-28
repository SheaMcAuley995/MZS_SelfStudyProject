
using UnityEngine;

public class TerrainGen : MonoBehaviour
{

    public int width = 256;
    public int height = 256;

    public int depth = 20;
    public float scale = 20;

    float offsetX = 10000;
    float offsetY = 10000;
    public float timer = 2;

    private void Start()
    {
        offsetX = Random.Range(1, 100000);
        offsetY = Random.Range(1, 100000);
    }

    void Update()
    {
        Terrain terr = GetComponent<Terrain>();
        terr.terrainData = GenerateTerrain(terr.terrainData);
        offsetX += Time.deltaTime * timer;
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
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
