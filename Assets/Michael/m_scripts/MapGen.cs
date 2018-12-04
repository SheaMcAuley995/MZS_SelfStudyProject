using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap,FallOffMap, DrawMesh };
    public DrawMode drawMode;

    public Noise.NormalizeMode normalizeMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int editorlevelOfDetail;
    //public int mapChunkSize, mapChunkSize;
    public float noiseScale;
    public int octaves;
    [Range(0,1)]
    public float persisence;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public float meshHeight;
    public AnimationCurve meshHeightCurve;

    public bool useFalloff;

    public bool autoUpdate;
    public TerrainTypes[] regions;
    float[,] falloffMap;


    Queue<MapThreadInfo<MapData>> MapDataInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> MeshDataInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private void Awake()
    {
        falloffMap = FalloffMapGenerator.GenerateFalloffMapp(mapChunkSize);
    }

    public void DrawMapInEditor()
    {
        MapData mapdata = GenThatMapData(Vector2.zero);
        Map map = FindObjectOfType<Map>();
        if (drawMode == DrawMode.NoiseMap)
        {
            map.DrawTexture(TextureGen.TextureFromHeightMap(mapdata.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            map.DrawTexture(TextureGen.TextureFromColorMap(mapdata.colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.DrawMesh)
        {
            map.DrawMesh(MeshGen.GennerateTerrainMesh(mapdata.heightMap, meshHeight, meshHeightCurve, editorlevelOfDetail), TextureGen.TextureFromColorMap(mapdata.colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.FallOffMap)
        {
            map.DrawTexture(TextureGen.TextureFromHeightMap(FalloffMapGenerator.GenerateFalloffMapp(mapChunkSize)));
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate { MapDataThread(center, callback); };
        new Thread(threadStart).Start();
    }
    void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenThatMapData(center);
        lock (MapDataInfoQueue) { MapDataInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData)); }
        
       
    }
    public void RequestMeshData(MapData mapData,int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate {
            MeshDataThread(mapData, lod, callback);
        };
        new Thread(threadStart).Start();
    }
    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGen.GennerateTerrainMesh(mapData.heightMap, meshHeight, meshHeightCurve, lod);
        lock (MeshDataInfoQueue)
        { MeshDataInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));}

    }

    private void Update()
    {
        if (MapDataInfoQueue.Count >0)
        {
            for (int i = 0; i < MapDataInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = MapDataInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
        if (MeshDataInfoQueue.Count > 0)
        {
            for (int i = 0; i < MeshDataInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = MeshDataInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.param);
            }
        }
    }


    MapData GenThatMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize,seed , noiseScale,octaves,persisence,lacunarity,center + offset, normalizeMode);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;


                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap);
    }

    private void OnValidate()
    {
        
        if (lacunarity<1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        falloffMap = FalloffMapGenerator.GenerateFalloffMapp(mapChunkSize);
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T param;

        public MapThreadInfo(Action<T> callback, T param)
        {
            this.callback = callback;
            this.param = param;
        }
    }


}
[System.Serializable]
public struct TerrainTypes {
    public float height;
    public string name;
    public Color color;

}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}

