using UnityEngine;

public class ChunkFactory : Singleton<ChunkFactory>
{
    public UniverseChunk SpawnChunk(Vector3 position, UniverseType universe)
    {
        var scriptable = ResourceSystem.instance.GetUniverse(universe);

        var chunk = Instantiate(scriptable.Chunks[Random.Range(0, scriptable.Chunks.Count)], position, Quaternion.identity);

        //add extra stuf to chunk?

        return chunk;
    }
    public UniverseChunk SpawnStartChunk(Vector3 position, UniverseType universe)
    {
        var scriptable = ResourceSystem.instance.GetUniverse(universe);

        var chunk = Instantiate(scriptable.Chunks[Random.Range(0, scriptable.Chunks.Count)], position, Quaternion.identity);

        //add extra stuf to chunk?

        return chunk;
    }
}
