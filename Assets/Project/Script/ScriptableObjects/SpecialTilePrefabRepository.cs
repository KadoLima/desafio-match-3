using Gazeus.DesafioMatch3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialTilePrefabRepository", menuName = "Gameplay/SpecialTilePrefabRepository")]
    public class SpecialTilePrefabRepository : ScriptableObject
    {
        [System.Serializable]
        public struct SpecialTileEntry
        {
            public SpecialType type;
            public GameObject prefab;
        }

        [SerializeField] private SpecialTileEntry[] _specialTileEntries;

        private Dictionary<SpecialType, GameObject> _specialTilesDict;

        private void Init()
        {
            _specialTilesDict = new Dictionary<SpecialType, GameObject>();

            for (int i = 0; i < _specialTileEntries.Length; i++)
            {
                SpecialTileEntry entry = _specialTileEntries[i];

                if (!_specialTilesDict.ContainsKey(entry.type))
                {
                    _specialTilesDict.Add(entry.type, entry.prefab);
                }
            }
        }

        public GameObject GetPrefab(SpecialType type)
        {
            if (_specialTilesDict == null) 
            { 
                Init();
            } 

            if (_specialTilesDict.TryGetValue(type, out var prefab)) return prefab;

            Debug.LogWarning($"[SpecialTileRepository] No prefab was set for {type}");
            return null;
        }
    }
}
