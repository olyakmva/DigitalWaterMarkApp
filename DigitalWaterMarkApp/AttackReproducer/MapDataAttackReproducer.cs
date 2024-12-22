using System.Numerics;
using DotSpatial.Projections;
using SupportLib;

namespace DigitalWaterMarkApp {

   public static class AttackRepropducer {

        /// <summary>
        /// Удаляет случайным образом заданный процент объектов в исходном слое
        /// Возвращает копию слоя
        /// </summary>
        /// <param name="percentage">Процент объектов для удаления</param>
        /// <param name="mapData">Исходный слой</param>
        public static MapData DropRandomPercentageOfData(double percentage, MapData mapData, bool changeOriginal = false) {

            if (!changeOriginal) {
                mapData = new() {
                    MapObjDictionary = new Dictionary<int, List<MapPoint>>(mapData.MapObjDictionary),
                    ColorName = mapData.ColorName.Copy()
                };
            }

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

        /// <summary>
        /// Атака путем перемешивания объектов внутри слоя карты
        /// </summary>
        //public static (double similiarObjectsPercentage, MapData shufflingMapData) ShuffleObjectsInMap(MapData mapData, bool changeOriginal = false) {

        //    if (!changeOriginal) {
        //        mapData = new() {
        //            MapObjDictionary = new List<KeyValuePair<int, List<MapPoint>>>(mapData.MapObjDictionary),
        //            ColorName = mapData.ColorName.Copy()
        //        };
        //    }

        //    var mapDataObjectIds = mapData.MapObjDictionary.Select(mapObject => mapObject.Key).ToList();
        //    Random random = new(Guid.NewGuid().GetHashCode());
        //    int n = mapData.ObjectsCount;
        //    while (n-- > 1) {
        //        int k = random.Next(n + 1);
        //        mapData.SwapMapObjectsById(mapDataObjectIds[k], mapDataObjectIds[n]);
        //    }

        //    float similiarObjectsCount = 0;
        //    for (int i = 0; i < mapData.ObjectsCount; i++) {
        //        var mapDataObjectId = mapData.MapObjDictionary[i].Key;
        //        var shufflingMapDataObjectId = mapData.MapObjDictionary[i].Key;

        //        if (mapDataObjectId == shufflingMapDataObjectId) {
        //            similiarObjectsCount++;
        //        }
        //    }

        //    var similiarObjectsPercentage = Math.Round(similiarObjectsCount / (float) mapData.ObjectsCount * 100.0F, 2);
        //    return (similiarObjectsPercentage, mapData);
        //}

    }

}