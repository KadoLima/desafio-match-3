using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "GeneralGameRules", menuName = "Gameplay/GeneralGameRules")]
    public class GeneralGameRulesSO : ScriptableObject
    {
        [Header("GENERAL")]
        [SerializeField] private int _pointsPerTile = 20;

        [Header("SPECIAL TILES SETTINGS")]
        [SerializeField] private int _maxSpecialTilesOnBoard = 3;
        [SerializeField] private float _chanceToSpawnSpecialTile = 0.1f;

        [Header("HINT SETTINGS")]
        [SerializeField] private bool _showHint = true;
        [SerializeField] private float _hintIdleSeconds = 3f;

        public int PointsPerTile => _pointsPerTile;
        public int MaxSpecialTilesOnBoard => _maxSpecialTilesOnBoard;
        public float ChanceToSpawnSpecialTile => _chanceToSpawnSpecialTile;
        public float HintIdleSeconds => _hintIdleSeconds;
        public bool ShowHint => _showHint;
    }
}
