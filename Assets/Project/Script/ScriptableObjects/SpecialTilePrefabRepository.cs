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

        [SerializeField] private SpecialTileEntry[] _entries;

        private Dictionary<SpecialType, GameObject> _map;

        private void Init()
        {
            _map = new Dictionary<SpecialType, GameObject>();

            for (int i = 0; i < _entries.Length; i++)
            {
                SpecialTileEntry entry = _entries[i];

                if (!_map.ContainsKey(entry.type))
                {
                    _map.Add(entry.type, entry.prefab);
                }
            }
        }

        public GameObject GetPrefab(SpecialType type)
        {
            if (_map == null) 
            { 
                Init();
            } 

            if (_map.TryGetValue(type, out var prefab)) return prefab;

            Debug.LogWarning($"[SpecialTileRepository] No prefab was set for {type}");
            return null;
        }
    }
}
