using System.Globalization;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SupportLib;

namespace DigitalWaterMarkApp
{
    public class MapDataProcessor
    {
        private WaterMark waterMark;

        // rlhlin1000.shp A=61 B=54 P=11 M=9

        private static BigInteger A = 35;
        private static BigInteger B = 84;

        private static BigInteger P = 95;
        private static BigInteger M = 4;

        public WaterMark WaterMark {
            get => waterMark;
            set => waterMark = value;
        }

        public MapDataProcessor(WaterMark waterMark) {
            this.waterMark = waterMark;
        }

        public MapData WaterMarkEmbedding(MapData mapData) {

            for (int i = 0; i < mapData.MapObjDictionary.Count - 1; i++)
            {
                var waterMarkEmbeddingItemIdx = GetHash(mapData.MapObjDictionary[i].Value.Count * mapData.MapObjDictionary[i + 1].Value.Count);
                var waterMarkEmbeddingItem = this.waterMark[waterMarkEmbeddingItemIdx];
                var storageDirection = GetMapObjectsStorageDirtection(mapData.MapObjDictionary[i].Value, mapData.MapObjDictionary[i + 1].Value);

                if (waterMarkEmbeddingItem != storageDirection) {
                    mapData.DuplicatePointInObjectByIndexAtPosition(i, waterMarkEmbeddingItemIdx);
                    mapData.SwapMapObjects(i, i + 1);
                }
            }

            return mapData;
        }

        public static int[] WaterMarkExtracting(MapData mapData) {

            Dictionary<int, List<int>> waterMarkValues = new();
            for (int i = 0; i < mapData.MapObjDictionary.Count - 1; i++)
            {
                var waterMarkExtractionItemIdx = GetHash(mapData.MapObjDictionary[i].Value.Count * mapData.MapObjDictionary[i + 1].Value.Count);
                var waterMarkExtractionItem = GetMapObjectsStorageDirtection(mapData.MapObjDictionary[i].Value, mapData.MapObjDictionary[i + 1].Value);

                if (waterMarkValues.ContainsKey(waterMarkExtractionItemIdx)) {
                    waterMarkValues[waterMarkExtractionItemIdx].Add(waterMarkExtractionItem);
                } else {
                    waterMarkValues.Add(waterMarkExtractionItemIdx, new List<int>(waterMarkExtractionItem));
                }
            }

            var maxIndex = waterMarkValues.Keys.Max();
            var waterMarkBestVariation = Enumerable.Repeat(-1, maxIndex + 1).ToArray();
            foreach (var wmKey in waterMarkValues.Keys)
            {
                var countOf_1 = waterMarkValues[wmKey].Count(item => item == 1);
                var countOf_0 = waterMarkValues[wmKey].Count(item => item == 0);

                if (countOf_1 > countOf_0) waterMarkBestVariation[wmKey] = 1;
                else if (countOf_0 > countOf_1) waterMarkBestVariation[wmKey] = 0;
            }

            return waterMarkBestVariation;
        }

        private static void LoopDuplicatingPointsAtIndex(int position, int objectId, MapData mapData) {

            if (position == 0) {
                return;
            }

            int[] positionsForDuplicating = new int[mapData[objectId].Count / position];
            int currentIndex = 0;
            for (int i = position; i < mapData[objectId].Count + currentIndex && currentIndex < positionsForDuplicating.Length; i += position) {
                positionsForDuplicating[currentIndex] = i - 1 + currentIndex++;
            }

            for (int i = 0; i < positionsForDuplicating.Length; i++) {
                mapData[objectId].Insert(positionsForDuplicating[i], mapData[objectId][positionsForDuplicating[i]]);
            }
        }

        public void LoopDuplicatingPointsInLayers(MapData mapData) {
            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                int countPointsInObject = mapObject.Value.Count;
                int wmDecimal = this.waterMark.ConvertToDecimal();

                int periodAsPosition = countPointsInObject % wmDecimal;
                LoopDuplicatingPointsAtIndex(periodAsPosition, objectId, mapData);
                Console.WriteLine(String.Format("Result of looping for object {0}: before {1} after {2}", objectId, countPointsInObject, mapData[objectId].Count));
            }
        }

        private static int GetHash(int value) => (int) (((A * value + B) % P) % M);

        private static int GetMapObjectsStorageDirtection(
            List<MapPoint> firstMapObject,
            List<MapPoint> secondMapObject
        ) {
            return firstMapObject.Count <= secondMapObject.Count ? 0 : 1;
        }
    }
}