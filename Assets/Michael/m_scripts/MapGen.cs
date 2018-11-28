﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int mapWidth, mapHeight;
    public float noiseScale;
    public bool autpUpdate;
    public int octaves;
    [Range(0,1)]
    public float persisence;
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    public void GenThatMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight,seed , noiseScale,octaves,persisence,lacunarity, offset);

        Map map = FindObjectOfType<Map>();
        map.DrawNoiseMap(noiseMap);
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
