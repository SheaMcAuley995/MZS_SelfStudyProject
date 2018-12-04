using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public enum NormalizeMode {Local,Global };
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight,int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        System.Random prng = new System.Random(seed);
        Vector2[] octOffset = new Vector2[octaves];
        float maxPossibleHeight = 0;
        float amp = 1;
        float freq = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octOffset[i] = new Vector2(offsetX, offsetY);
            maxPossibleHeight += amp;
            amp += persistance;
        }

        if (scale <= 0)
        {
            scale = 0.00001f;
        }

        float maxLocalNoise = float.MinValue;
        float minLocalNoise = float.MaxValue;
        float halfW = mapWidth / 2f;
        float halfH = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                amp = 1;
                freq = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfW + octOffset[i].x) / scale * freq;
                    float sampleY = (y - halfH + octOffset[i].y) / scale * freq;
                    float perlinVal = Mathf.PerlinNoise(sampleX, sampleY)*2-1;
                    noiseHeight += perlinVal * amp;

                    amp *= persistance;
                    freq *= lacunarity;
                }
                if (noiseHeight > maxLocalNoise)
                {
                    maxLocalNoise = noiseHeight;
                }
                else if (noiseHeight < minLocalNoise)
                {
                    minLocalNoise = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoise, maxLocalNoise, noiseMap[x, y]);
                }
                else
                {
                    float normalizedHight = noiseMap[x, y] + 1 / (2f * maxPossibleHeight/ 1.3f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHight, 0 , int.MaxValue);
                }
                
            }
        }
                return noiseMap;
    }
}
