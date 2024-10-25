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

        private static int A = 61;
        private static int B = 54;

        private static int P = 11;
        private static int M = 4;

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
                var waterMarkEmbeddingItemIdx = GetHash(mapData[i].Count * mapData[i + 1].Count);
                var waterMarkEmbeddingItem = this.waterMark[waterMarkEmbeddingItemIdx];
                var storageDirection = GetMapObjectsStorageDirtection(mapData[i], mapData[i + 1]);

                if (waterMarkEmbeddingItem != storageDirection) {
                    mapData.DuplicatePointInObjectByIndexAtPosition(i, waterMarkEmbeddingItemIdx);
                    mapData.SwapMapObjects(i, i + 1);
                }
            }

            return mapData;
        }

        public int[] WaterMarkExtracting(MapData mapData) {

            Dictionary<int, List<int>> waterMarkValues = new();
            for (int i = 0; i < mapData.MapObjDictionary.Count - 1; i++)
            {
                var waterMarkExtractionItemIdx = GetHash(mapData[i].Count * mapData[i + 1].Count);
                var waterMarkExtractionItem = GetMapObjectsStorageDirtection(mapData[i], mapData[i + 1]);

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

        private int GetHash(int value) => ((A * value + B) % P) % M;

        private int GetMapObjectsStorageDirtection(
            List<MapPoint> firstMapObject,
            List<MapPoint> secondMapObject
        ) {
            return firstMapObject.Count <= secondMapObject.Count ? 0 : 1;
        }
    }
}