using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Universe", menuName = "Create New Universe")]
public class UniverseSO : ScriptableObject
{
    public UniverseType UniverseType;
    public ParallaxBackground ParallaxBackground;
    public StartChunk StartChunk;
    public List<UniverseChunk> Chunks;
}

[Serializable]
public enum UniverseType
{
    Overworld,
    Nether,
    End
}

