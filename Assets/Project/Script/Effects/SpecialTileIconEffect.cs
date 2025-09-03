using UnityEngine;
using DG.Tweening;

namespace Gazeus.DesafioMatch3.Effects
{
    public class SpecialTileIconEffect : MonoBehaviour
    {
        [Header("SETTINGS")]
        [Tooltip("Scale multiplier applied on X or Y axis during the stretch effect.")]
        [SerializeField] private float _stretchAmount = 1.1f;
        [Tooltip("Duration in seconds of each stretch or return animation step.")]
        [SerializeField] private float _duration = 0.3f; 

        private Tween _loopTween;

        private void OnEnable()
        {
            PlayEffect();
        }

        private void OnDisable()
        {
            _loopTween?.Kill();
        }

        private void PlayEffect()
        {
            transform.localScale = Vector3.one;

            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOScale(new Vector3(_stretchAmount, 1f, 1f), _duration))
               .Append(transform.DOScale(Vector3.one, _duration));

            seq.Append(transform.DOScale(new Vector3(1f, _stretchAmount, 1f), _duration))
               .Append(transform.DOScale(Vector3.one, _duration));

            seq.SetLoops(-1);

            _loopTween = seq;
        }
    }
}
