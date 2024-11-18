using System.Numerics;
using DotSpatial.Projections;
using SupportLib;

namespace DigitalWaterMarkApp.AttackReproducer.Reproducers {

   public class DroppingAttackReproducer : IAttackReproducer {

      /// <summary>
      /// Удаляет случайным образом заданный процент объектов в исходном слое
      /// Возвращает копию слоя
      /// </summary>
      /// <param name="percentage">Процент объектов для удаления</param>
      /// <param name="mapData">Исходный слой</param>
      public MapData RunAttack(MapData mapData, Dictionary<string, object> parameters) {

         mapData =  new()
         {
            MapObjDictionary = new List<KeyValuePair<int, List<MapPoint>>>(mapData.MapObjDictionary),
            ColorName = mapData.ColorName.Copy()
         };

         var countObjectsForDropping = (int) (mapData.ObjectsCount * (double) parameters["percentage"]);

         var mapDataObjectIds = mapData.MapObjDictionary.Select(mapObject => mapObject.Key).ToList();
         Random random = new(Guid.NewGuid().GetHashCode());
         while (countObjectsForDropping > 0) {
            var indexOfMapDataObjectId = random.Next(0, mapDataObjectIds.Count);
            mapData.Remove(mapDataObjectIds[indexOfMapDataObjectId]);
            mapDataObjectIds.RemoveAt(indexOfMapDataObjectId);
            countObjectsForDropping--;
         }

         return mapData;
      }
   }
}