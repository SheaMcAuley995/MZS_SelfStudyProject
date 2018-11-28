using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, DrawMesh };
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
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

    public bool autoUpdate;
    public TerrainTypes[] regions;

    Queue<MapThreadInfo<MapData>> MapDataInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> MeshDataInfoQueue = new Queue<MapThreadInfo<MeshData>>();
    public void DrawMapInEditor()
    {
        MapData mapdata = GenThatMapData();
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
            map.DrawMesh(MeshGen.GennerateTerrainMesh(mapdata.heightMap, meshHeight, meshHeightCurve, levelOfDetail), TextureGen.TextureFromColorMap(mapdata.colorMap, mapChunkSize, mapChunkSize));
        }
    }

    public void RequestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate { MapDataThread(callback); };
        new Thread(threadStart).Start();
    }
    void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenThatMapData();
        lock (MapDataInfoQueue) { MapDataInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData)); }
        
       
    }
    public void RequestMeshData(MapData mapData, Action<MeshData> callback)
    {

    }
    void MeshDataThread(MapData mapData, Action<MeshData> callback)
    {
        MeshData meshData = MeshGen.GennerateTerrainMesh(mapData.heightMap, meshHeight, meshHeightCurve, levelOfDetail);
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


    MapData GenThatMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize,seed , noiseScale,octaves,persisence,lacunarity, offset);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;

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

