using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int madWidth, int mapHeight, float scale)
    {
        float[,] noiseMap = new float[madWidth, mapHeight];
        if (scale <= 0)
        {
            scale = 0.00001f;
        }
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < madWidth; x++)
            {
                float sampleX = x/scale;
                float sampleY = y/scale;
                float perlinVal = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinVal;

            }
        }
        return noiseMap;
    }
}
