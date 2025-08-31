using Gazeus.DesafioMatch3.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Interfaces
{
    public interface ISpecialEffect
    {
        List<Vector2Int> GetClearedPositions(List<List<Tile>> board, Vector2Int from, Vector2Int to);
    }
}
