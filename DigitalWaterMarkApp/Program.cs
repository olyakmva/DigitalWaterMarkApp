using System.Linq;
// Для пакетной обработки
//using DotSpatial.Data;
using DigitalWaterMarkApp;
using SupportLib;

class Program
{
    public static void PrintWaterMark(WaterMark waterMark)
    {
        for (int i = 0; i < waterMark.Size * waterMark.Size; i++)
        {
            Console.Write(waterMark[i] + " ");
        }
        Console.WriteLine();
    }

    public static void PrintWaterMark(int[] waterMark)
    {
        for (int i = 0; i < waterMark.Length; i++)
        {
            Console.Write(waterMark[i] + " ");
        }
        Console.WriteLine();
    }

    public static void Main()
    {
        /**
        * rivers1000k.shp
        * rlhlin1000.shp
        */

        ShapeFileIO shapeFileIO = new();
        MapData mapData = shapeFileIO.Open("test1/rivers1000k.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;

        // foreach (var mapDataObject in mapData)
        // {
        //     Console.WriteLine(String.Format("WM-Object key: {0}, vertices count {1}, {2}", mapDataObject.Key, mapDataObject.Value.Count, mapData.HasDuplicatedPoints(mapDataObject.Key)));
        // }

        for (int i = 30; i < 31; i++)
        {
            WaterMark waterMark = WaterMark.ConvertToWaterMark(i);
            PrintWaterMark(waterMark);

            MapDataProcessor mapDataProcessor = new(waterMark);
            mapDataProcessor.LoopDuplicatingPointsInLayers(mapData);

            var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
            var extractedWM = MapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark, waterMark.Length);
            PrintWaterMark(extractedWM);

            Console.WriteLine("-------------------");
        }
    }
}

