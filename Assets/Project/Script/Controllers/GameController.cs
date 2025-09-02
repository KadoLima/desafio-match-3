using DG.Tweening;
using Gazeus.DesafioMatch3.Core;
using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.ScriptableObjects;
using Gazeus.DesafioMatch3.Views;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class GameController : MonoBehaviour
    {
        [Header("BOARD")]
        [SerializeField] private BoardView _boardView;
        [SerializeField] private int _boardHeight = 10;
        [SerializeField] private int _boardWidth = 10;

        [Header("SCORE REWARD")]
        [SerializeField] private CurrenciesController _currenciesController;
        [SerializeField] private PlayerCurrencySO _matchRewardCurrency;

        [Header("GAME RULES")]
        [SerializeField] private GeneralGameRulesSO _generalGameRules;

        [Space(10)]
        [SerializeField] private UnityEvent<float> ValidMatchEvent;

        private GameService _gameService;
        private bool _isAnimating;
        private int _selectedX = -1;
        private int _selectedY = -1;
        private bool _isPlayable = false;

        private float _idleTimer;
        private Vector3 _lastMousePos;
        private bool _hintShown;

        #region Unity
        private void Awake()
        {
            _gameService = new GameService();
            _gameService.SetupGameRules(_generalGameRules);

            SpecialTileEffectsRegistry.Register(SpecialType.CLEAR_LINE, new ClearLineEffect());
            SpecialTileEffectsRegistry.Register(SpecialType.COLOR_BOMB, new ColorBombEffect());

            _boardView.TileClicked += OnTileClick;
            _boardView.FinishedCreatingVisualBoard += OnFinishedCreatingVisualBoard;
        }

        private void OnDestroy()
        {
            _boardView.TileClicked -= OnTileClick;
        }

        private void Start()
        {
            List<List<Tile>> board = _gameService.StartGame(_boardWidth, _boardHeight);
            _boardView.CreateBoard(board);
        }

        private void Update()
        {
            ShowHintIfPossible();
        }
        #endregion

        private void AnimateBoard(List<BoardSequence> boardSequences, int index, Action onComplete)
        {
            BoardSequence boardSequence = boardSequences[index];

            HandleMatchReward(boardSequence.MatchedPosition);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_boardView.DestroyTiles(boardSequence.MatchedPosition, boardSequence.DestroyBySpecial));
            sequence.Append(_boardView.MoveTiles(boardSequence.MovedTiles));
            sequence.Append(_boardView.CreateTile(boardSequence.AddedTiles));

            index += 1;
            if (index < boardSequences.Count)
            {
                sequence.onComplete += () => AnimateBoard(boardSequences, index, onComplete);
            }
            else
            {
                sequence.onComplete += () => onComplete();
            }
        }

        private void HandleMatchReward(List<Vector2Int> matchedPosition)
        {
            ValidMatchEvent.Invoke(matchedPosition.Count);
            _currenciesController.AddAmount(_matchRewardCurrency, matchedPosition.Count);
        }

        private void OnTileClick(int x, int y)
        {
            if (!_isPlayable) return;

            if (_isAnimating) return;

            if (_selectedX > -1 && _selectedY > -1)
            {
                // Ex: Tile (7,4) clicado primeiro. Tile (7,5) clicado segundo.
                // (7,4) -> (7,5) 
                // |7-7| + |4-5| = 0 + 1 = 1 portanto ADJACENTE. 
                //Distância Manhattan em Grids.
                // Ex: Tile (7,4) clicado primeiro. Tile (2,5) clicado segundo.
                // (7,4) -> (2,5) 
                // |7-2| + |4-5| = 5 + 1 = 6 portanto NÃO ADJACENTE. 
                //Distância Manhattan em Grids.
                //|fromX - toX| + |fromY - toY| == 1
                if (Mathf.Abs(_selectedX - x) + Mathf.Abs(_selectedY - y) > 1) //NÃO ADJACENTE!
                {
                    _boardView.DeselectSelected();
                    _selectedX = -1;
                    _selectedY = -1;
                }
                else
                {
                    ResetIdleAndClearHint();

                    _isAnimating = true;
                    _boardView.SwapTiles(_selectedX, _selectedY, x, y).onComplete += () =>
                    {
                        bool isValid = _gameService.IsValidMovement(_selectedX, _selectedY, x, y);
                        if (isValid)
                        {
                            List<BoardSequence> swapResult = _gameService.SwapTile(_selectedX, _selectedY, x, y);

                            AnimateBoard(swapResult, 0, () =>
                            {
                                _boardView.DeselectSelected();
                                _isAnimating = false;
                            });
                        }
                        else
                        {
                            _boardView.SwapTiles(x, y, _selectedX, _selectedY).onComplete += () =>
                            {
                                _boardView.DeselectSelected();                                  
                                _isAnimating = false;
                            };
                        }
                        _selectedX = -1;
                        _selectedY = -1;
                    };
                }
            }
            else
            {
                _selectedX = x;
                _selectedY = y;
                _boardView.SelectTile(x, y);
            }
        }

        private void OnFinishedCreatingVisualBoard()
        {
            TogglePlayable(true);
        }

        private void TogglePlayable(bool newState)
        {
            _isPlayable = newState;
        }

        private void ShowHintIfPossible()
        {
            if (!_isPlayable) return;

            if (!_generalGameRules.ShowHint) return;

            if (_isAnimating)
            {
                ResetIdleAndClearHint();
                _idleTimer = 0f;
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                ResetIdleAndClearHint();
                _idleTimer = 0f;
                return;
            }

            _idleTimer += Time.deltaTime;

            if (!_hintShown && _idleTimer >= _generalGameRules.HintIdleSeconds && _selectedX == -1 && _selectedY == -1)
            {
                if (_gameService.TryFindHint(out var from))
                {
                    _boardView.ClearAllHints();
                    _boardView.ShowHintAt(from);
                    _hintShown = true;
                }
            }
        }

        private void ResetIdleAndClearHint()
        {
            _idleTimer = 0f;
            if (_hintShown)
            {
                _boardView.ClearAllHints();
                _hintShown = false;
            }
        }
    }
}