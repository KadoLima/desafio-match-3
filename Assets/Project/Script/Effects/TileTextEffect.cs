using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Gazeus.DesafioMatch3.Effects
{
    public class TileTextEffect : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textEffect;

        private void Awake()
        {
            Hide();
        }

        public void Show()
        {
            //_textEffect.text = "+99";
            //_textEffect.gameObject.SetActive(true);

            //_textEffect.transform.localScale = Vector3.one;
            //_textEffect.transform.localPosition = Vector3.zero; 
            //_textEffect.alpha = 1f;

            //_textEffect.transform.DOScale(2f, 0.2f);

            //_textEffect.transform.DOLocalMoveY(50f, 0.5f).SetRelative().SetEase(Ease.OutQuad);

            //_textEffect.DOFade(0f, 0.5f).SetEase(Ease.InQuad).OnComplete(Hide);
        }

        public void Hide()
        {
            _textEffect.gameObject.SetActive(false);
        }
    }
}
