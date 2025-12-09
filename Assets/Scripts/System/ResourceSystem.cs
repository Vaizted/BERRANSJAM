using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
   // public List<> Units { get; private set; }
   // private Dictionary<UnitType, ScriptableUnitBase> UnitDict;
   //
   // protected override void Awake()
   // {
   //     base.Awake();
   //     AssembleResources();
   // }
   //
   // private void AssembleResources()
   // {
   //     Units = Resources.LoadAll<ScriptableUnitBase>("Cows").ToList();
   //     UnitDict = Units.ToDictionary(r => r.UnitType, r => r);
   // }
   // 
   // public ScriptableUnitBase GetUnit(UnitType type) => UnitDict[type];
   // public ScriptableUnitBase GetRandomUnit() => Units[Random.Range(0, Units.Count)];
   //
}
