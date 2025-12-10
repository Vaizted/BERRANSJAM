using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UniverseManager : Singleton<UniverseManager>
{
    [SerializeField] private UniverseType StartUniverse;
    [SerializeField] private UniverseChunk StartChunk;
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private float chunkSize = 40f;

    [Header("Chunk Settings")]
    [SerializeField] private float spawnAhead = 40f;
    [SerializeField] private float despawnBehind = 80f;

    private Queue<UniverseChunk> activeChunks = new Queue<UniverseChunk>();
    float nextSpawnX = 0f;
    
    private GameObject player;
    private GameObject currentParallax;
    private UniverseType currentUniverse;
    private bool generate = false;

    public void StartGenerate()
    {
        currentUniverse = StartUniverse;

        SpawnStartChunk();

        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).gameObject;
        FindFirstObjectByType<CameraFollow>().SetTarget(player.transform);

        currentParallax = Instantiate(ResourceSystem.instance.GetUniverse(currentUniverse).ParallaxBackground, new Vector3(0, 3.5f, 0), Quaternion.identity).gameObject;

        generate = true;
    }

    private void Update()
    {
        if (!generate)
            return;

        if (player.transform.position.x + spawnAhead > nextSpawnX)
        {
            SpawnChunk();
        }

        if (activeChunks.Count > 0)
        {
            var first = activeChunks.Peek();
            if (player.transform.position.x - first.transform.position.x > despawnBehind)
            {
                DespawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        var chunk = ChunkFactory.instance.SpawnChunk(new Vector3(nextSpawnX, 0, 0), currentUniverse);
        activeChunks.Enqueue(chunk);
        nextSpawnX += chunkSize;
    }
    void SpawnStartChunk()
    {
        var chunk = ChunkFactory.instance.SpawnStartChunk(new Vector3(nextSpawnX, 0, 0), currentUniverse);
        activeChunks.Enqueue(chunk);
        nextSpawnX += chunkSize;
    }

    void DespawnChunk()
    {
        Destroy(activeChunks.Dequeue().gameObject);
    }

    public void SwapUniverse(UniverseType universe)
    {
        currentUniverse = universe;

        foreach (var c in activeChunks)
        {
            Destroy(c.gameObject);
        }
        activeChunks.Clear();

        nextSpawnX = player.transform.position.x;
        SpawnStartChunk();

        Destroy(currentParallax);
        currentParallax = Instantiate(ResourceSystem.instance.GetUniverse(currentUniverse).ParallaxBackground, player.transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity).gameObject;
    }

    public void StopGenerate()
    {
        generate = false;
    }
}
