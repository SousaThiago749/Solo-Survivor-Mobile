using UnityEngine;
using System.Collections.Generic;

public class InfiniteTilemap : MonoBehaviour{
    [Header("Chunks disponíveis")]
    public GameObject[] chunkPrefabs;  // Agora é um array de prefabs diferentes

    [Header("Referências")]
    public Transform player;

    [Header("Tamanho dos Chunks")]
    public int chunkWidth = 16;
    public int chunkHeight = 10;
    public int renderRadius = 2;

    private Vector2Int lastPlayerChunk = Vector2Int.zero;
    private Dictionary<Vector2Int, GameObject> spawnedChunks = new Dictionary<Vector2Int, GameObject>();



    void Update(){
        Vector2Int currentChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkWidth),
            Mathf.FloorToInt(player.position.y / chunkHeight)
        );

        if (currentChunk != lastPlayerChunk)
        {
            GenerateAroundPlayer(currentChunk);
            CleanupDistantChunks(currentChunk);
            lastPlayerChunk = currentChunk;
        }
    }

    void GenerateAroundPlayer(Vector2Int centerChunk){
        for (int x = -renderRadius; x <= renderRadius; x++)
        {
            for (int y = -renderRadius; y <= renderRadius; y++)
            {
                Vector2Int chunkCoord = new Vector2Int(centerChunk.x + x, centerChunk.y + y);

                if (!spawnedChunks.ContainsKey(chunkCoord))
                {
                    Vector3 spawnPosition = new Vector3(
                        chunkCoord.x * chunkWidth,
                        chunkCoord.y * chunkHeight,
                        0f
                    );

                    GameObject selectedPrefab = GetRandomChunkPrefab();
                    GameObject newChunk = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedChunks.Add(chunkCoord, newChunk);
                }
            }
        }
    }

    GameObject GetRandomChunkPrefab(){
        if (chunkPrefabs.Length == 0)
        {
            Debug.LogWarning("Nenhum chunk prefab disponível!");
            return null;
        }

        int randomIndex = Random.Range(0, chunkPrefabs.Length);
        return chunkPrefabs[randomIndex];
    }

void CleanupDistantChunks(Vector2Int centerChunk){
    List<Vector2Int> chunksToRemove = new List<Vector2Int>();

    foreach (var kvp in spawnedChunks)
    {
        Vector2Int chunkCoord = kvp.Key;

        int distanceX = Mathf.Abs(chunkCoord.x - centerChunk.x);
        int distanceY = Mathf.Abs(chunkCoord.y - centerChunk.y);

        if (distanceX > renderRadius || distanceY > renderRadius)
        {
            chunksToRemove.Add(chunkCoord);
        }
    }

    foreach (Vector2Int coord in chunksToRemove)
    {
        Destroy(spawnedChunks[coord]);
        spawnedChunks.Remove(coord);
    }
}


}
