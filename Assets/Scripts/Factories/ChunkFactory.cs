using UnityEngine;

public class ChunkFactory : Singleton<ChunkFactory>
{
    public UniverseChunk SpawnRandomChunk(Vector3 position, UniverseType universe)
    {
        var scriptable = ResourceSystem.instance.GetUniverse(universe);

        var chunk = Instantiate(scriptable.Chunks[Random.Range(0, scriptable.Chunks.Count)], position, Quaternion.identity);

        //add extra stuf to chunk?

        return chunk;
    }
    public StartChunk SpawnStartChunk(Vector3 position, UniverseType universe)
    {
        var scriptable = ResourceSystem.instance.GetUniverse(universe);

        var chunk = Instantiate(scriptable.StartChunk, position, Quaternion.identity);

        //add extra stuf to chunk?

        return chunk;
    }
}
