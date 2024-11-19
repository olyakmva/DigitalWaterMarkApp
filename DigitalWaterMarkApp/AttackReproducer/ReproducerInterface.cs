
using System.Numerics;
using DotSpatial.Projections;
using SupportLib;

namespace DigitalWaterMarkApp.AttackReproducer {
    public interface IAttackReproducer {
        string GetName() => GetType().Name;
        MapData RunAttack(MapData mapData, Dictionary<string, object> parameters);
    }
}

