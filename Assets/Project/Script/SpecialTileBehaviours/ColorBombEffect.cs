using Gazeus.DesafioMatch3.Interfaces;
using Gazeus.DesafioMatch3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class ColorBombEffect : ISpecialEffect
    {
        public List<Vector2Int> GetClearedPositions(List<List<Tile>> board, Vector2Int from, Vector2Int to)
        {
            Vector2Int bombPos = from;
            Vector2Int neighborPos = to;

            Tile bombTile = board[bombPos.y][bombPos.x];
            Tile neighborTile = board[neighborPos.y][neighborPos.x];

            if (bombTile.SpecialType != SpecialType.COLOR_BOMB && neighborTile.SpecialType == SpecialType.COLOR_BOMB)
            {
                bombPos = to;
                neighborPos = from;
                bombTile = board[bombPos.y][bombPos.x];
                neighborTile = board[neighborPos.y][neighborPos.x];
            }

            if (neighborTile.SpecialType != SpecialType.NONE || neighborTile.Type < 0) return new List<Vector2Int>(); 
            

            int targetType = neighborTile.Type;

            List<Vector2Int> cleared = new List<Vector2Int>();
            int height = board.Count;
            int width = board[0].Count;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var t = board[y][x];

                    if (t.SpecialType == SpecialType.NONE && t.Type == targetType)
                    {
                        cleared.Add(new Vector2Int(x, y));
                    }
                }
            }

            if (!cleared.Contains(bombPos))
            {
                cleared.Add(bombPos);
            }

            if (!cleared.Contains(neighborPos))
            {
                cleared.Add(neighborPos);
            }

            return cleared;
        }
    }
}
