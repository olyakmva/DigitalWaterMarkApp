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
                var waterMarkEmddingItem = this.waterMark[GetHash(mapData[i].Count * mapData[i + 1].Count)];
                var storageDirection = GetMapObjectsStorageDirtection(mapData[i], mapData[i + 1]);

                if (waterMarkEmddingItem != storageDirection) {
                    mapData.SwapMapObjects(i, i + 1);
                }
            }

            return mapData;
        }

        private int GetHash(int value) {
            return value % 16;
        }

        private int GetMapObjectsStorageDirtection(
            List<MapPoint> firstMapObject,
            List<MapPoint> secondMapObject
        ) {
            return firstMapObject.Count <= secondMapObject.Count ? 0 : 1;
        }
    }
}