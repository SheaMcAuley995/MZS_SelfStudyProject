using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap };
    public DrawMode drawMode;
    public int mapWidth, mapHeight;
    public float noiseScale;
    public bool autpUpdate;
    public int octaves;
    [Range(0,1)]
    public float persisence;
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    public TerrainTypes[] regions;

    public void GenThatMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed , noiseScale,octaves,persisence,lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;

                        break;
                    }
                }
            }
        }
        Map map = FindObjectOfType<Map>();
        if (drawMode == DrawMode.NoiseMap)
        {
            map.DrawTexture(TextureGen.TextureFromHeightMap(noiseMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            map.DrawTexture(TextureGen.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
    }

    private void OnValidate()
    {
        if (mapWidth<1)
        {
            mapWidth = 1;

        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity<1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }

}
[System.Serializable]
public struct TerrainTypes {
    public float height;
    public string name;
    public Color color;

}
