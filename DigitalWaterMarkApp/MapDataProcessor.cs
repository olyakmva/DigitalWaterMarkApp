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
                var waterMarkEmbeddingItemIdx = GetHash(mapData.MapObjDictionary[i].Value.Count * mapData.MapObjDictionary[i + 1].Value.Count, this.WaterMark.Length);
                var waterMarkEmbeddingItem = this.waterMark[waterMarkEmbeddingItemIdx];
                var storageDirection = GetMapObjectsStorageDirtection(mapData.MapObjDictionary[i].Value, mapData.MapObjDictionary[i + 1].Value);

                if (waterMarkEmbeddingItem != storageDirection) {
                    mapData.DuplicatePointInObjectByIndexAtPosition(i, waterMarkEmbeddingItemIdx);
                    mapData.SwapMapObjects(i, i + 1);
                }
            }

            return mapData;
        }

        public static int[] WaterMarkExtracting(MapData mapData, int waterMarkLength) {

            Dictionary<int, List<int>> waterMarkValues = new();
            for (int i = 0; i < mapData.MapObjDictionary.Count - 1; i++)
            {
                var waterMarkExtractionItemIdx = GetHash(mapData.MapObjDictionary[i].Value.Count * mapData.MapObjDictionary[i + 1].Value.Count, waterMarkLength);
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
            var maximumPossibleWMvalue = -1;
            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                int countPointsInObject = mapObject.Value.Count;
                int wmDecimal = this.waterMark.ConvertToDecimal();

                int periodAsPosition = countPointsInObject % wmDecimal;

                if (periodAsPosition > 0 && periodAsPosition > maximumPossibleWMvalue) {
                    maximumPossibleWMvalue = periodAsPosition;
                }

                LoopDuplicatingPointsAtIndex(periodAsPosition, objectId, mapData);
                // Console.WriteLine(String.Format("Result of looping for object {0}: before {1} after {2}", objectId, countPointsInObject, mapData[objectId].Count));
            }
            Console.WriteLine(String.Format("Maximum possible WM value {0}", maximumPossibleWMvalue));
        }

        private static List<int> FindLoopingPositionsInLayer(int objectId, MapData mapData) {

            List<int> duplicatingIndexes = new();

            for (int i = 1; i < mapData[objectId].Count; i++) {
                var previousItem = mapData[objectId][i - 1];
                var currentItem = mapData[objectId][i];

                if (previousItem.CompareTo(currentItem) == 0) {
                    duplicatingIndexes.Add(i - 1);
                }
            }

            return duplicatingIndexes;
        }

        private static int FindDifferenceForLooping(int objectId, MapData mapData) {
            var loopingIndexes = FindLoopingPositionsInLayer(objectId, mapData);
            var countPointsBeforeLooping = mapData[objectId].Count - loopingIndexes.Count;

            if (loopingIndexes.Count == 0) {
                return -1;
            }

            var loopingPosition = loopingIndexes[0] + 1;

            if (loopingPosition == 1) {
                return -1;
            }

            // Console.WriteLine(String.Format("Current quiv: {0}(mod X)={1}", countPointsBeforeLooping, loopingPosition));

            return countPointsBeforeLooping - loopingPosition;
        }

        private static List<int> FindAllDifferencesInLayers(MapData mapData) {
            List<int> mapLayerLoopingDifferences = new();
            foreach (var mapObject in mapData) {
                int objectId = mapObject.Key;
                var currentDefference = FindDifferenceForLooping(objectId, mapData);
                if (currentDefference != -1) {
                    mapLayerLoopingDifferences.Add(currentDefference);
                }
            }
            return mapLayerLoopingDifferences;
        }

        private static int GCD(int a, int b) {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a);
        }

        private static int GCDOfList(List<int> numbers) {
            if (numbers == null || numbers.Count == 0)
                throw new ArgumentException("List cannot be null or empty.");

            int gcdResult = numbers[0];

            for (int i = 1; i < numbers.Count; i++)
            {
                gcdResult = GCD(gcdResult, numbers[i]);
                if (gcdResult == 1)
                    return 1;
            }

            return gcdResult;
        }

        public static WaterMark FindWMDecimalFromLoopingsInMapData(MapData mapData) {
            List<int> loopingDifferences = FindAllDifferencesInLayers(mapData);
            return WaterMark.ConvertToWaterMark(GCDOfList(loopingDifferences));
        }

        private static int GetHash(int value, int waterMarkLength) => (int) (((A * value + B) % P) % waterMarkLength);

        private static int GetMapObjectsStorageDirtection(
            List<MapPoint> firstMapObject,
            List<MapPoint> secondMapObject
        ) {
            return firstMapObject.Count <= secondMapObject.Count ? 0 : 1;
        }
    }
}