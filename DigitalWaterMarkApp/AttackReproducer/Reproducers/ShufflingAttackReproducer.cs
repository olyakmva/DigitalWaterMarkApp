using System.Numerics;
using DotSpatial.Projections;
using SupportLib;

namespace DigitalWaterMarkApp.AttackReproducer.Reproducers {

    public class ShufflingAttackReproducer : AttackReproducerBase {

      public override MapData RunAttack(MapData mapData, Dictionary<string, object> parameters) {

         MapData mapDataCopy =  new() {
            MapObjDictionary = new List<KeyValuePair<int, List<MapPoint>>>(mapData.MapObjDictionary),
            ColorName = mapData.ColorName.Copy()
         };

         var mapDataObjectIds = mapDataCopy.MapObjDictionary.Select(mapObject => mapObject.Key).ToList();
         Random random = new(Guid.NewGuid().GetHashCode());
         int n = mapDataCopy.ObjectsCount;
         while (n-- > 1) {
            int k = random.Next(n + 1);
            mapDataCopy.SwapMapObjectsById(mapDataObjectIds[k], mapDataObjectIds[n]);
         }

         float similiarObjectsCount = 0;
         for (int i = 0; i < mapData.ObjectsCount; i++) {
            var mapDataObjectId = mapData.MapObjDictionary[i].Key;
            var shufflingMapDataObjectId = mapDataCopy.MapObjDictionary[i].Key;

            if (mapDataObjectId == shufflingMapDataObjectId) {
               similiarObjectsCount++;
            }
         }

         var similiarObjectsPercentage = Math.Round(similiarObjectsCount / (float) mapData.ObjectsCount * 100.0F, 2);
         return mapDataCopy;
      }

   }

}