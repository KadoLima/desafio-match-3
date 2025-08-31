using Gazeus.DesafioMatch3.Interfaces;
using Gazeus.DesafioMatch3.Models;
using System.Collections.Generic;

namespace Gazeus.DesafioMatch3.Core

{
    public static class SpecialTileEffectsRegistry
    {
        private static readonly Dictionary<SpecialType, ISpecialEffect> _map = new();


        public static void Register(SpecialType type, ISpecialEffect effect)
        {
            _map[type] = effect;
        }

        public static bool TryGet(SpecialType type, out ISpecialEffect effect) => _map.TryGetValue(type, out effect);
    }
}
