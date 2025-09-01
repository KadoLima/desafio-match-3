using Gazeus.DesafioMatch3.Interfaces;
using Gazeus.DesafioMatch3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core
{
    public class GameService
    {
        private List<List<Tile>> _boardTiles;
        private List<int> _tilesTypes;
        private int _tileCount;
        private int _specialCount;

        private int _maxSpecials;
        private float _specialChance;

        public void SetupGameRules(GeneralGameRulesSO rules)
        {
            _maxSpecials = rules.MaxSpecialTilesOnBoard;
            _specialChance = rules.ChanceToSpawnSpecialTile;
        }

        public bool IsValidMovement(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            var a = newBoard[fromY][fromX];
            var b = newBoard[toY][toX];

            if (a.SpecialType != SpecialType.NONE || b.SpecialType != SpecialType.NONE)
            {
                ISpecialEffect effect = null;

                if (!SpecialTileEffectsRegistry.TryGet(a.SpecialType, out effect))
                {
                    SpecialTileEffectsRegistry.TryGet(b.SpecialType, out effect);
                }

                if (effect != null)
                {
                    List<Vector2Int> cleared = effect.GetClearedPositions(newBoard, new Vector2Int(fromX, fromY), new Vector2Int(toX, toY));

                    if (cleared != null && cleared.Count > 0) return true;
                }
            }

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (x > 1 &&
                        newBoard[y][x].SpecialType == SpecialType.NONE &&
                        newBoard[y][x - 1].SpecialType == SpecialType.NONE &&
                        newBoard[y][x - 2].SpecialType == SpecialType.NONE &&
                        newBoard[y][x].Type == newBoard[y][x - 1].Type &&
                        newBoard[y][x - 1].Type == newBoard[y][x - 2].Type)
                    {
                        return true;
                    }

                    if (y > 1 &&
                        newBoard[y][x].SpecialType == SpecialType.NONE &&
                        newBoard[y - 1][x].SpecialType == SpecialType.NONE &&
                        newBoard[y - 2][x].SpecialType == SpecialType.NONE &&
                        newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                        newBoard[y - 1][x].Type == newBoard[y - 2][x].Type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<List<Tile>> StartGame(int boardWidth, int boardHeight)
        {
            _specialCount = 0;

            _tilesTypes = new List<int> { 0, 1, 2, 3 };
            _boardTiles = CreateBoard(boardWidth, boardHeight, _tilesTypes);

            return _boardTiles;
        }

        public List<BoardSequence> SwapTile(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            List<BoardSequence> sequences = new List<BoardSequence>();

            Tile a = newBoard[fromY][fromX];
            Tile b = newBoard[toY][toX];

            if (TryGetSpecialEffect(a, b, out var effect))
            {
                List<Vector2Int> cleared = effect.GetClearedPositions(newBoard, new Vector2Int(fromX, fromY), new Vector2Int(toX, toY));

                SpecialType cause = (a.SpecialType != SpecialType.NONE) ? a.SpecialType : b.SpecialType;

                BoardSequence sequence = MakeSequence(newBoard, cleared, cause);
                sequences.Add(sequence);

                var matched = FindMatches(newBoard);

                while (HasMatch(matched))
                {
                    var clearedMatch = new List<Vector2Int>();
                    for (int y = 0; y < newBoard.Count; y++)
                    {
                        for (int x = 0; x < newBoard[y].Count; x++)
                        {
                            if (matched[y][x]) clearedMatch.Add(new Vector2Int(x, y));
                        }
                    }

                    BoardSequence sequenceCascade = MakeSequence(newBoard, clearedMatch);
                    sequences.Add(sequenceCascade);

                    matched = FindMatches(newBoard);
                }

                _boardTiles = newBoard;
                return sequences;
            }

            var matchedTiles = FindMatches(newBoard);

            while (HasMatch(matchedTiles))
            {
                var cleared = new List<Vector2Int>();
                for (int y = 0; y < newBoard.Count; y++)
                {
                    for (int x = 0; x < newBoard[y].Count; x++)
                    {
                        if (matchedTiles[y][x]) cleared.Add(new Vector2Int(x, y));
                    }
                }

                var sequence = MakeSequence(newBoard, cleared);
                sequences.Add(sequence);

                matchedTiles = FindMatches(newBoard);
            }

            _boardTiles = newBoard;
            return sequences;
        }

        private List<List<Tile>> CopyBoard(List<List<Tile>> boardToCopy)
        {
            List<List<Tile>> newBoard = new(boardToCopy.Count);
            for (int y = 0; y < boardToCopy.Count; y++)
            {
                newBoard.Add(new List<Tile>(boardToCopy[y].Count));
                for (int x = 0; x < boardToCopy[y].Count; x++)
                {
                    Tile tile = boardToCopy[y][x];
                    newBoard[y].Add(new Tile { Id = tile.Id, Type = tile.Type, SpecialType = tile.SpecialType });
                }
            }

            return newBoard;
        }

        private List<List<Tile>> CreateBoard(int width, int height, List<int> tileTypes)
        {
            List<List<Tile>> board = new(height);
            _tileCount = 0;
            for (int y = 0; y < height; y++)
            {
                board.Add(new List<Tile>(width));
                for (int x = 0; x < width; x++)
                {
                    board[y].Add(new Tile { Id = -1, Type = -1 });
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<int> noMatchTypes = new(tileTypes.Count);
                    for (int i = 0; i < tileTypes.Count; i++)
                    {
                        noMatchTypes.Add(_tilesTypes[i]);
                    }

                    if (x > 1 &&
                        board[y][x - 1].Type == board[y][x - 2].Type)
                    {
                        noMatchTypes.Remove(board[y][x - 1].Type);
                    }

                    if (y > 1 &&
                        board[y - 1][x].Type == board[y - 2][x].Type)
                    {
                        noMatchTypes.Remove(board[y - 1][x].Type);
                    }

                    board[y][x].Id = _tileCount++;
                    board[y][x].Type = noMatchTypes[Random.Range(0, noMatchTypes.Count)];
                    board[y][x].SpecialType = SpecialType.NONE;

                    if (_specialCount < _maxSpecials && Random.value < _specialChance)
                    {
                        board[y][x].SpecialType = GetRandomSpecialType();
                        _specialCount++;
                    }
                }
            }

            return board;
        }

        private static List<List<bool>> FindMatches(List<List<Tile>> newBoard)
        {
            List<List<bool>> matchedTiles = new();

            for (int y = 0; y < newBoard.Count; y++)
            {
                matchedTiles.Add(new List<bool>(newBoard[y].Count));
                for (int x = 0; x < newBoard.Count; x++)
                {
                    matchedTiles[y].Add(false);
                }
            }

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (x > 1 &&
                        newBoard[y][x].SpecialType == SpecialType.NONE &&
                        newBoard[y][x - 1].SpecialType == SpecialType.NONE &&
                        newBoard[y][x - 2].SpecialType == SpecialType.NONE &&
                        newBoard[y][x].Type == newBoard[y][x - 1].Type &&
                        newBoard[y][x - 1].Type == newBoard[y][x - 2].Type)
                    {
                        matchedTiles[y][x] = true;
                        matchedTiles[y][x - 1] = true;
                        matchedTiles[y][x - 2] = true;
                    }

                    if (y > 1 &&
                        newBoard[y][x].SpecialType == SpecialType.NONE &&
                        newBoard[y - 1][x].SpecialType == SpecialType.NONE &&
                        newBoard[y - 2][x].SpecialType == SpecialType.NONE &&
                        newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                        newBoard[y - 1][x].Type == newBoard[y - 2][x].Type)
                    {
                        matchedTiles[y][x] = true;
                        matchedTiles[y - 1][x] = true;
                        matchedTiles[y - 2][x] = true;
                    }
                }
            }

            return matchedTiles;
        }

        private bool TryGetSpecialEffect(Tile a, Tile b, out ISpecialEffect effect)
        {
            if (SpecialTileEffectsRegistry.TryGet(a.SpecialType, out effect)) return true;
            if (SpecialTileEffectsRegistry.TryGet(b.SpecialType, out effect)) return true;

            effect = null;

            return false;
        }

        private void ClearPositions(List<List<Tile>> board, List<Vector2Int> positions)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                var p = positions[i];
                if (board[p.y][p.x].SpecialType != SpecialType.NONE)
                {
                    _specialCount = Mathf.Max(0, _specialCount - 1);
                }

                board[p.y][p.x] = new Tile { Id = -1, Type = -1, SpecialType = SpecialType.NONE };
            }
        }

        private List<MovedTileInfo> DropColumns(List<List<Tile>> board, List<Vector2Int> cleared)
        {
            var movedDict = new Dictionary<int, MovedTileInfo>();
            var movedList = new List<MovedTileInfo>();

            for (int i = 0; i < cleared.Count; i++)
            {
                int x = cleared[i].x;
                int y = cleared[i].y;

                if (y > 0)
                {
                    for (int j = y; j > 0; j--)
                    {
                        Tile movedTile = board[j - 1][x];
                        board[j][x] = movedTile;

                        if (movedTile.Type > -1)
                        {
                            if (movedDict.TryGetValue(movedTile.Id, out var info))
                            {
                                info.To = new Vector2Int(x, j);
                            }
                            else
                            {
                                var infoNew = new MovedTileInfo
                                {
                                    From = new Vector2Int(x, j - 1),
                                    To = new Vector2Int(x, j)
                                };

                                movedDict[movedTile.Id] = infoNew;
                                movedList.Add(infoNew);
                            }
                        }
                    }

                    board[0][x] = new Tile { Id = -1, Type = -1, SpecialType = SpecialType.NONE };
                }
            }

            return movedList;
        }

        private List<AddedTileInfo> FillEmpties(List<List<Tile>> board)
        {
            var added = new List<AddedTileInfo>();

            for (int y = board.Count - 1; y >= 0; y--)
            {
                for (int x = board[y].Count - 1; x >= 0; x--)
                {
                    if (board[y][x].Type == -1)
                    {
                        int typeIndex = Random.Range(0, _tilesTypes.Count);
                        var t = board[y][x];
                        t.Id = _tileCount++;
                        t.Type = _tilesTypes[typeIndex];
                        t.SpecialType = SpecialType.NONE;

                        if (_specialCount < _maxSpecials && Random.value < _specialChance)
                        {
                            t.SpecialType = GetRandomSpecialType();

                            if (t.SpecialType != SpecialType.NONE)
                            {
                                _specialCount++;
                            }
                        }

                        added.Add(new AddedTileInfo
                        {
                            Position = new Vector2Int(x, y),
                            Type = t.Type,
                            SpecialType = t.SpecialType
                        });
                    }
                }
            }

            return added;
        }

        private BoardSequence MakeSequence(List<List<Tile>> board, List<Vector2Int> clearedPositions)
        {
            ClearPositions(board, clearedPositions);

            List<MovedTileInfo> moved = DropColumns(board, clearedPositions);
            List<AddedTileInfo> added = FillEmpties(board);

            return new BoardSequence
            {
                MatchedPosition = clearedPositions,
                MovedTiles = moved,
                AddedTiles = added
            };
        }

        private BoardSequence MakeSequence(List<List<Tile>> board, List<Vector2Int> clearedPositions, SpecialType cause)
        {
            ClearPositions(board, clearedPositions);

            List<MovedTileInfo> moved = DropColumns(board, clearedPositions);
            List<AddedTileInfo> added = FillEmpties(board);

            Dictionary<Vector2Int, SpecialType> destroyMap = new Dictionary<Vector2Int, SpecialType>(clearedPositions.Count);

            for (int i = 0; i < clearedPositions.Count; i++)
            {
                destroyMap[clearedPositions[i]] = cause;
            }

            return new BoardSequence
            {
                MatchedPosition = clearedPositions,
                MovedTiles = moved,
                AddedTiles = added,
                DestroyBySpecial = destroyMap
            };
        }

        private bool HasMatch(List<List<bool>> list)
        {
            for (int y = 0; y < list.Count; y++)
            {
                for (int x = 0; x < list[y].Count; x++)
                {
                    if (list[y][x])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private SpecialType GetRandomSpecialType()
        {
            var values = (SpecialType[])System.Enum.GetValues(typeof(SpecialType));

            List<SpecialType> validSpecialTileTypes = new List<SpecialType>();

            for (int i = 0; i < values.Length; i++)
            {
                SpecialType specialType = values[i];

                if (specialType != SpecialType.NONE)
                {
                    validSpecialTileTypes.Add(specialType);
                }
            }

            int index = Random.Range(0, validSpecialTileTypes.Count);

            return validSpecialTileTypes[index];
        }
    }
}
