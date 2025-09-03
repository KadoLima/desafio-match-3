using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class MouseCursorController : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private Texture2D _cursorTexture;
        [SerializeField] private PlayerInput playerInput;

        [Header("SETTINGS")]
        [SerializeField] private Vector2 _clickPosition = Vector2.zero;

        private InputAction _reloadAction;
        private const string RELOAD_ACTION = "Reload";

        #region Unity

        private void OnEnable()
        {
            _reloadAction = playerInput.actions[RELOAD_ACTION];

            if (_reloadAction != null)
            {
                _reloadAction.performed += OnReload;
                _reloadAction.Enable();
            }
        }

        private void OnDisable()
        {
            if (_reloadAction != null)
            {
                _reloadAction.performed -= OnReload;
                _reloadAction.Disable();
                _reloadAction = null;
            }
        }

        private void Start()
        {
            Cursor.SetCursor(_cursorTexture, _clickPosition, CursorMode.Auto);
        }

        #endregion

        public void OnReload(InputAction.CallbackContext callback)
        {
            if (callback.performed)
            {
                DOTween.KillAll(true);   
                DOTween.Clear(true);     
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
