using System.Numerics;
using SupportLib;

namespace DigitalWaterMarkApp {

   public static class AttackRepropducer {

      /// <summary>
      /// Удаляет случайным образом заданный процент объектов в исходном слое
      /// Возвращает копию слоя
      /// </summary>
      /// <param name="percentage">Процент объектов для удаления</param>
      /// <param name="mapData">Исходный слой</param>
      public static MapData DropRandomPercentageOfData(double percentage, MapData mapData) {
         mapData = mapData.Copy();
         var countObjectsForDropping = (int) (mapData.ObjectsCount * percentage);

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