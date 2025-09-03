using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.UI
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;

        [Header("DEFAULT BACKGROUND SETTINGS")]
        [Tooltip("Default horizontal scroll speed of the background texture.")]
        [SerializeField] private float _x;
        [Tooltip("Default vertical scroll speed of the background texture.")]
        [SerializeField] private float _y;

        [Header("SPECIAL SETTINGS")]
        [Tooltip("Color applied to the background during special mode.")]
        [SerializeField] private Color _specialColor;
        [Tooltip("Horizontal scroll speed of the background texture during special mode.")]
        [SerializeField] private float _specialX;
        [Tooltip("Vertical scroll speed of the background texture during special mode.")]
        [SerializeField] private float _specialY;

        private float _defaultX;
        private float _defaultY;
        private Color _defaultColor;
        private Vector2 _currentScrollingSpeed;

        #region Unity
        private void Awake()
        {
            _defaultX = _x;
            _defaultY = _y;
            _defaultColor = _rawImage.color;

            ResetColorAndSpeed();
        }

        void Update()
        {
            _rawImage.uvRect = new Rect(_rawImage.uvRect.position + _currentScrollingSpeed * Time.deltaTime, _rawImage.uvRect.size);
        }
        #endregion

        public void ChangeColorAndSpeed()
        {
            _currentScrollingSpeed = new Vector2(_specialX, _specialY);
            _rawImage.color = _specialColor;
        }

        public void ResetColorAndSpeed()
        {
            _currentScrollingSpeed = new Vector2(_defaultX, _defaultY);
            _rawImage.color = _defaultColor;
        }
    }
}
