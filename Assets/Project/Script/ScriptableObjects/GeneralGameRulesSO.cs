using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "GeneralGameRules", menuName = "Gameplay/GeneralGameRules")]
    public class GeneralGameRulesSO : ScriptableObject
    {
        [Header("GENERAL")]
        [Tooltip("Gold amount awarded for each tile destroyed.")]
        [SerializeField] private int _goldPerTile = 10;

        [Header("SPECIAL TILES SETTINGS")]
        [Tooltip("Maximum number of special tiles allowed simultaneously on the board.")]
        [SerializeField] private int _maxSpecialTilesOnBoard = 3;
        [Tooltip("Chance for a destroyed tile to spawn as a special tile.")]
        [SerializeField] private float _chanceToSpawnSpecialTile = 0.1f;

        [Header("HINT SETTINGS")]
        [Tooltip("If enabled, the game shows hints after the player is idle.")]
        [SerializeField] private bool _showHint = true;
        [Tooltip("Seconds of player inactivity before a hint is shown.")]
        [SerializeField] private float _hintIdleSeconds = 3f;

        [Header("SPECIAL METER SETTINGS")]
        [Tooltip("Percentage of the special meter filled per tile destroyed.")]
        [SerializeField] private float _fillPercentPerTileDestroyed = 5f;
        [Tooltip("Duration in seconds that the special mode remains active once triggered.")]
        [SerializeField] private float _specialDurationSeconds = 120f;
        [Tooltip("Score multiplier applied while the special mode is active.")]
        [SerializeField] private float _specialMeterMultiplier = 2f;

        public int GoldPerTile => _goldPerTile;
        public int MaxSpecialTilesOnBoard => _maxSpecialTilesOnBoard;
        public float ChanceToSpawnSpecialTile => _chanceToSpawnSpecialTile;
        public float HintIdleSeconds => _hintIdleSeconds;
        public bool ShowHint => _showHint;
        public float FillPercentPerTileDestroyed => _fillPercentPerTileDestroyed;
        public float SpecialDurationSeconds => _specialDurationSeconds;
        public float SpecialMeterMultiplier => _specialMeterMultiplier;
    }
}
