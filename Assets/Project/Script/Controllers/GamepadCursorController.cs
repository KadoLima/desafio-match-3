using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class GamepadCursorController : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private RectTransform _cursorTransform;
        [SerializeField] private RectTransform _canvasRectTransform;

        [Header("SETTINGS")]
        [Tooltip("Speed multiplier that controls how fast the virtual cursor moves when using the gamepad stick.")]
        [SerializeField] private float _cursorSpeed = 1000f;
        [Tooltip("Padding in screen pixels to keep the virtual cursor away from the screen edges.")]
        [SerializeField] private float _padding = 50f;

        private Mouse _virtualMouse;
        private Mouse _currentMouse;
        private bool _previousMouseState;
        private Camera _mainCamera;

        private string _previousControlScheme;

        private const string VIRTUAL_MOUSE_DEVICE = "VirtualMouse";
        private const string GAMEPAD_SCHEME = "Gamepad";
        private const string MOUSE_SCHEME = "Keyboard&Mouse";

        #region Unity

        private void Awake()
        {
            _mainCamera = Camera.main;
            _currentMouse = Mouse.current;
        }

        private void Start()
        {
            ApplyControlScheme(MOUSE_SCHEME);
        }

        private void OnEnable()
        {
            if (_virtualMouse == null)
            {
                _virtualMouse = (Mouse)InputSystem.AddDevice(VIRTUAL_MOUSE_DEVICE);
            }
            else if (!_virtualMouse.added)
            {
                InputSystem.AddDevice(_virtualMouse);
            }

            InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);

            if (_cursorTransform != null)
            {
                Vector2 position = new Vector2(Screen.width/2, Screen.height/2);
                InputState.Change(_virtualMouse.position, position);
            }

            InputSystem.onAfterUpdate += UpdateMotion;

            _playerInput.onControlsChanged += OnControlsChanged;
        }

        private void OnDisable()
        {
            InputSystem.onAfterUpdate -= UpdateMotion;
            _playerInput.onControlsChanged -= OnControlsChanged;

            if (_virtualMouse != null)
            {
                InputUser.PerformPairingWithDevice(_virtualMouse, default);
            }

            if (_virtualMouse != null && _virtualMouse.added)
            {
                InputSystem.RemoveDevice(_virtualMouse);
            }

            _virtualMouse = null;
        }

        #endregion

        private void UpdateMotion()
        {
            if (_virtualMouse == null || Gamepad.current == null) return;

            Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
            stickValue *= _cursorSpeed * Time.deltaTime;

            Vector2 currentPosition = _virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + stickValue;

            newPosition.x = Mathf.Clamp(newPosition.x, _padding, Screen.width - _padding);
            newPosition.y = Mathf.Clamp(newPosition.y, _padding, Screen.height - _padding);

            InputState.Change(_virtualMouse.position, newPosition);
            InputState.Change(_virtualMouse.delta, stickValue);

            bool aButtonIsPressed = Gamepad.current.buttonSouth.IsPressed();

            if (_previousMouseState != aButtonIsPressed)
            {
                _virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(_virtualMouse, mouseState);
                _previousMouseState = aButtonIsPressed;
            }

            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 position)
        {
            Vector2 anchoredPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, position, _mainCamera, out anchoredPosition);

            _cursorTransform.anchoredPosition = anchoredPosition;
        }

        private void OnControlsChanged(PlayerInput input)
        {
            if (input.currentControlScheme == _previousControlScheme) return;
            if (_virtualMouse == null || !_virtualMouse.added) return;

            ApplyControlScheme(input.currentControlScheme);
            _previousControlScheme = input.currentControlScheme;
        }

        private void ApplyControlScheme(string scheme)
        {
            if (scheme == MOUSE_SCHEME)
            {
                _cursorTransform.gameObject.SetActive(false);
                Cursor.visible = true;

                if (_currentMouse != null && _virtualMouse != null && _virtualMouse.added)
                {
                    _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                }
            }
            else if (scheme == GAMEPAD_SCHEME)
            {
                _cursorTransform.gameObject.SetActive(true);
                Cursor.visible = false;

                if (_virtualMouse != null && _virtualMouse.added && _currentMouse != null)
                {
                    InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
                    AnchorCursor(_currentMouse.position.ReadValue());
                }
            }
        }
    }
}
