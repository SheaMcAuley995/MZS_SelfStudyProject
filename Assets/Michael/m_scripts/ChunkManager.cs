using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    const float scale = 5;
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float squareViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;

    public LODInfo[] detailLevels;
    public static float maxViewDist;

    public Transform player;
    static MapGen mapGen;
    public Material mapMat;
    public static Vector2 playerPos;
    Vector2 playerPosOld;
    int chunkSize;
    int visibleChunks;
    public Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGen = FindObjectOfType<MapGen>();
        maxViewDist = detailLevels[detailLevels.Length - 1].distThreshold;
        chunkSize = MapGen.mapChunkSize - 1;
        visibleChunks = Mathf.RoundToInt(maxViewDist / chunkSize);
        UpdateVisibleChunks();
    }
    private void Update()
    {
        playerPos = new Vector2(player.position.x, player.position.z)/scale;
        if ((playerPosOld - playerPos).sqrMagnitude > squareViewerMoveThresholdForChunkUpdate)
        {
            playerPosOld = playerPos;
            UpdateVisibleChunks();
        }
        
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
                    
                }
                else {
                    terrainChunkDictionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,chunkSize,detailLevels, transform, mapMat));
                }
            }
        }
    }
    public class TerrainChunk
    {
        GameObject meshObj;
        SpawnPointsOnMesh spawnPoints;
        Vector2 pos;
        Bounds bounds;
        MapData mapData;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        bool mapDataRecieved;
        int prevLOD = -1;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material mat)
        {
            this.detailLevels = detailLevels;
            pos = coord * size;
            bounds = new Bounds(pos, Vector2.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);
            meshObj = new GameObject("Terrain Chunk");
            meshRenderer = meshObj.AddComponent<MeshRenderer>();
            meshFilter = meshObj.AddComponent<MeshFilter>();
            meshRenderer.material = mat;
            meshObj.transform.position = posV3*scale;           
            meshObj.transform.parent = parent;
            meshObj.transform.localScale = Vector3.one * scale;
            SetVisible(false);
            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; i++)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
            }
            mapGen.RequestMapData(pos, OnMapDataRecieved);

        }
        void OnMapDataRecieved(MapData mapData)
        {
            this.mapData = mapData;
            mapDataRecieved = true;
            Texture2D texture = TextureGen.TextureFromColorMap(mapData.colorMap, MapGen.mapChunkSize, MapGen.mapChunkSize);
            meshRenderer.material.mainTexture = texture;
            UpdateTerrainChunk();
        }
       
        public void UpdateTerrainChunk()
        {
            if (mapDataRecieved)
            {
                float viewDistFromEdge = Mathf.Sqrt(bounds.SqrDistance(playerPos));
                bool isVis = viewDistFromEdge <= maxViewDist;
                if (isVis)
                {
                    int lodIdx = 0;
                    for (int i = 0; i < detailLevels.Length - 1; i++)
                    {
                        if (viewDistFromEdge > detailLevels[i].distThreshold)
                        {
                            lodIdx = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (lodIdx != prevLOD)
                    {
                        LODMesh LODmesh = lodMeshes[lodIdx];
                        if (LODmesh.hasMesh)
                        {
                            prevLOD = lodIdx;
                            meshFilter.mesh = LODmesh.mesh;
                        }
                        else if (!LODmesh.hasRequestedMEsh)
                        {
                            LODmesh.RequestMesh(mapData);

                        }
                    }
                    terrainVisibleLastUpdate.Add(this);
                }
                SetVisible(isVis);
            }
            
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
    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMEsh;
        public bool hasMesh;
        int lod;
        System.Action updateCallBack;
        public LODMesh(int lod, System.Action updateCallBack)
        {
            this.lod = lod;
            this.updateCallBack = updateCallBack;
        }
        void OnMeshDataRecieved(MeshData meshData)
        {
            mesh = meshData.CreateMessh();
            hasMesh = true;
            updateCallBack();
        }
        public void RequestMesh(MapData mapData) {
            hasRequestedMEsh = true;
            mapGen.RequestMeshData(mapData, lod, OnMeshDataRecieved);
        }
    }
    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float distThreshold;
    }

}
