using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3.Effects
{
    public class BoardEffects : MonoBehaviour
    {
        [Header("SPAWN EFFECT SETTINGS")]
        [Tooltip("Initial time between each tile spawn.")]
        [SerializeField] private float _interval = 0.05f;
        [Tooltip("Minimum time allowed between tiles.")]
        [SerializeField] private float _minInterval = 0.01f;
        [Tooltip("Decay factor for interval between tiles.")]
        [SerializeField] private float _decay = 0.97f;
        [Tooltip("Duration of the tile spawn animation.")]
        [SerializeField] private float _tileSpawnDuration = 0.2f;

        [Space(10)]
        [Header("SHAKE EFFECT SETTINGS")]
        [Tooltip("Total duration of the board shake effect in seconds.")]
        [SerializeField] private float shakeDuration = 0.3f;
        [Tooltip("Maximum shake strength.")]
        [SerializeField] private float shakeStrength = 0.5f;
        [Tooltip("Number of vibrations during the effect.")]
        [SerializeField] private int vibrato = 10;

        [SerializeField] private UnityEvent FinishedInitialBoardEffectsEvent;
        
        private Ease _tileSpawnEase = Ease.OutBack;
        private Vector3 _originalPosition;
        private Tween _shakeTween;


        #region Unity
        private void Awake()
        {
            _originalPosition = transform.localPosition;
        }
        #endregion

        public void PlayBoardSpawnEffect(GameObject[][] tiles)
        {
            Sequence sequence = DOTween.Sequence();

            float currentDelay = _interval;
            float currentInterval = _interval;

            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles[y].Length; x++)
                {
                    GameObject tile = tiles[y][x];

                    if (tile == null) continue;

                    sequence.Insert(currentDelay,
                        tile.transform.DOScale(1f, _tileSpawnDuration).SetEase(_tileSpawnEase));

                    currentDelay += currentInterval;
                    currentInterval = Mathf.Max(currentInterval * _decay, _minInterval);
                }
            }

            sequence.OnComplete(() => FinishedInitialBoardEffectsEvent.Invoke());
            sequence.Play();
        }

        public void ShakeBoard()
        {
            transform.localPosition = _originalPosition;
            _shakeTween?.Kill();
            _shakeTween = transform.DOShakePosition(shakeDuration, shakeStrength, vibrato);
        }
    }
}