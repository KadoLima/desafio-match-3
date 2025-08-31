using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "GeneralGameRules", menuName = "Gameplay/GeneralGameRules")]
    public class GeneralGameRulesSO : ScriptableObject
    {
        [Header("GENERAL")]
        [SerializeField] private int _pointsPerTile = 20;

        [Header("SPECIAL TILES")]
        [SerializeField] private int _maxSpecialTilesOnBoard = 3;
        [SerializeField] private float _chanceToSpawnSpecialTile = 0.1f;

        public int PointsPerTile => _pointsPerTile;
        public int MaxSpecialTilesOnBoard => _maxSpecialTilesOnBoard;
        public float ChanceToSpawnSpecialTile => _chanceToSpawnSpecialTile;
    }
}
