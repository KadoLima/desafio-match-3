using Gazeus.DesafioMatch3.Interfaces;
using Gazeus.DesafioMatch3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class ClearLineEffect : ISpecialEffect
    {
        public List<Vector2Int> GetClearedPositions(List<List<Tile>> board, Vector2Int from, Vector2Int to)
        {
            bool horizontal = (from.y == to.y);
            int width = board[0].Count;
            int height = board.Count;

            var matched = new List<Vector2Int>();

            if (horizontal)
            {
                int y = from.y;
                for (int x = 0; x < width; x++) matched.Add(new Vector2Int(x, y));
            }
            else
            {
                int x = from.x;
                for (int y = 0; y < height; y++) matched.Add(new Vector2Int(x, y));
            }

            return matched;
        }
    }
}
