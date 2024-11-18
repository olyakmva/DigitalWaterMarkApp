
using System.Numerics;
using DotSpatial.Projections;
using SupportLib;

namespace DigitalWaterMarkApp.AttackReproducer.Reproducers {
   public interface IAttackReproducer {
      string GetName() => this.GetType().Name;
      MapData RunAttack(MapData mapData, Dictionary<string, object> parameters);
   }
}

