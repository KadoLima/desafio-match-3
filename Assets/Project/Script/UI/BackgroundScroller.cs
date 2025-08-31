using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3.UI
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private float _x;
        [SerializeField] private float _y;

        #region Unity
        void Update()
        {
            _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _rawImage.uvRect.size);
        }
        #endregion
    }
}
