using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    public const float maxViewDist = 450f;
    public Transform player;
    static MapGen mapGen;
    public Material mapMat;
    public static Vector2 playerPos;
    int chunkSize;
    int visibleChunks;
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainVisibleLastUpdate = new List<TerrainChunk>();
    private void Start()
    {
        mapGen = FindObjectOfType<MapGen>();
        chunkSize = MapGen.mapChunkSize - 1;
        visibleChunks = Mathf.RoundToInt(maxViewDist / chunkSize);
    }
    private void Update()
    {
        playerPos = new Vector2(player.position.x, player.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainVisibleLastUpdate.Count; i++)
        {
            terrainVisibleLastUpdate[i].SetVisible(false);
        }
        terrainVisibleLastUpdate.Clear();
        int currentChunkCoordX = Mathf.RoundToInt(playerPos.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(playerPos.y / chunkSize);
        for (int yOffset = -visibleChunks; yOffset <= visibleChunks; yOffset++)
        {
            for (int xOffset = -visibleChunks; xOffset <= visibleChunks; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
                    {
                        terrainVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCoord]);
                    }
                }
                else {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,chunkSize, transform, mapMat));
                }
            }
        }
    }
    public class TerrainChunk
    {
        GameObject meshObj;
        Vector2 pos;
        Bounds bounds;
        MapData mapData;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        public TerrainChunk(Vector2 coord, int size, Transform parent, Material mat)
        {
            pos = coord * size;
            bounds = new Bounds(pos, Vector2.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);
            meshObj = new GameObject("Terrain Chunk");
            meshRenderer = meshObj.AddComponent<MeshRenderer>();
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshRenderer.material = mat;
            meshObj.transform.position = posV3;           
            meshObj.transform.parent = parent;
            SetVisible(false);
            mapGen.RequestMapData(OnMapDataRecieved);

        }
        void OnMapDataRecieved(MapData mapData)
        {
            mapGen.RequestMeshData(mapData, OnMeshDataRecieved);
        }
        void OnMeshDataRecieved(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMessh();
        }
        public void UpdateTerrainChunk()
        {
          float viewDistFromEdge = Mathf.Sqrt (bounds.SqrDistance(playerPos));
            bool isVis = viewDistFromEdge <= maxViewDist;
            SetVisible(isVis);
        }
        public void SetVisible(bool visible)
        {
            meshObj.SetActive(visible);
        }
        public bool IsVisible()
        {
            return meshObj.activeSelf;
        }
    }
}
