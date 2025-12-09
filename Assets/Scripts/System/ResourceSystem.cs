using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<UniverseSO> Universes { get; private set; }
    private Dictionary<UniverseType, UniverseSO> UniverseDict;
   
    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }
   
    private void AssembleResources()
    {
        Universes = Resources.LoadAll<UniverseSO>("Universes").ToList();
        UniverseDict = Universes.ToDictionary(r => r.UniverseType, r => r);
    }
    
    public UniverseSO GetUniverse(UniverseType type) => UniverseDict[type];
    public UniverseSO GetRandomUniverse() => Universes[Random.Range(0, Universes.Count)];
   
}
