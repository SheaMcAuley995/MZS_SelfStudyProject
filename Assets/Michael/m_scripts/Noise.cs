using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight,int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);
        Vector2[] octOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.00001f;
        }

        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;
        float halfW = mapWidth / 2f;
        float halfH = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amp = 1;
                float freq = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfW) / scale * freq + octOffset[i].x;
                    float sampleY = (y - halfH) / scale * freq + octOffset[i].y;
                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY)*2-1;
                    noiseHeight += perlinVal * amp;

                    amp *= persistance;
                    freq *= lacunarity;
                }
                if (noiseHeight > maxNoise)
                {
                    maxNoise = noiseHeight;
                }
                else if (noiseHeight < minNoise)
                {
                    minNoise = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noiseMap[x, y]);
            }
        }
                return noiseMap;
    }
}
