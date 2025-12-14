using UnityEngine;

public class ChunkFactory : Singleton<ChunkFactory>
{
    public UniverseChunk SpawnRandomChunk(UniverseType universe)
    {
        UniverseSO scriptable = ResourceSystem.instance.GetUniverse(universe);

        UniverseChunk chunk = Instantiate(scriptable.Chunks[Random.Range(0, scriptable.Chunks.Count)]);

        //add extra stuf to chunk?

        return chunk;
    }
    public StartChunk SpawnStartChunk(UniverseType universe)
    {
        UniverseSO scriptable = ResourceSystem.instance.GetUniverse(universe);

        StartChunk chunk = Instantiate(scriptable.StartChunk, Vector3.zero, Quaternion.identity);

        //add extra stuf to chunk?

        return chunk;
    }
}
