using System.Collections.Generic;
using UnityEngine;

public class UniverseManager : Singleton<UniverseManager>
{
    [SerializeField] private UniverseType StartUniverse;
    [SerializeField] private PlayerController playerPrefab;

    [Header("Chunk Settings")]
    [SerializeField] private int chunksAhead = 2;
    [SerializeField] private int chunksBehind = 2;

    [Header("Parallax Settings")]
    [SerializeField] private float parallaxHeight = 3f;

    private Queue<UniverseChunk> activeChunks = new Queue<UniverseChunk>();
    private UniverseChunk lastChunk;

    private GameObject player;
    private GameObject currentChaser;
    private GameObject currentParallax;
    private UniverseType currentUniverse;
    private bool generate = false;

    public void StartGenerate()
    {
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).gameObject;
        FindFirstObjectByType<CameraFollow>().SetTarget(player.transform);

        CreateUniverse(StartUniverse);

        generate = true;
    }

    private void Update()
    {
        if (!generate)
            return;

        while (CountChunksAheadOfPlayer() < chunksAhead)
        {
            SpawnChunk();
        }

        while (CountChunksBehindPlayer() > chunksBehind)
        {
            UniverseChunk oldChunk = activeChunks.Dequeue();
            Destroy(oldChunk.gameObject);
        }
    }

    private int CountChunksAheadOfPlayer()
    {
        int count = 0;
        foreach (UniverseChunk chunk in activeChunks)
        {
            if (chunk.StartPoint.position.x > player.transform.position.x)
                count++;
        }
        return count;
    }

    private int CountChunksBehindPlayer()
    {
        int count = 0;
        foreach (UniverseChunk chunk in activeChunks)
        {
            if (chunk.EndPoint.position.x < player.transform.position.x)
                count++;
        }
        return count;
    }

    private void SpawnChunk()
    {
        var newChunk = ChunkFactory.instance.SpawnRandomChunk(currentUniverse);

        Vector3 spawnPosition = lastChunk.EndPoint.position - newChunk.StartPoint.position;
        newChunk.transform.position += spawnPosition;

        activeChunks.Enqueue(newChunk);
        lastChunk = newChunk;
    }

    private void SpawnStartChunk(UniverseSO universeSO)
    {
        var startChunk = ChunkFactory.instance.SpawnStartChunk(currentUniverse); 

        activeChunks.Enqueue(startChunk);
        lastChunk = startChunk;

        if (player != null)
        {
            player.transform.position = startChunk.PlayerSpawn.position;
        }

        if (currentChaser != null)
        {
            Destroy(currentChaser);
        }
        currentChaser = Instantiate(universeSO.universeChaser, startChunk.ChaserSpawn.position, Quaternion.identity).gameObject;
    }

    private void CreateParallax(UniverseSO universeSO)
    {
        if (currentParallax != null)
        {
            Destroy(currentParallax);
        }
        currentParallax = Instantiate(universeSO.ParallaxBackground, player.transform.position + new Vector3(0, parallaxHeight, 0), Quaternion.identity).gameObject;
    }

    public void SwapUniverse(UniverseType universe)
    {
        foreach (var c in activeChunks)
        {
            Destroy(c.gameObject);
        }
        activeChunks.Clear();

        CreateUniverse(universe);
    }

    private void CreateUniverse(UniverseType universe)
    {
        currentUniverse = universe;
        UniverseSO universeSO = ResourceSystem.instance.GetUniverse(currentUniverse);

        SpawnStartChunk(universeSO);
        CreateParallax(universeSO);
    }

    public void StopGenerate()
    {
        generate = false;
    }
}
