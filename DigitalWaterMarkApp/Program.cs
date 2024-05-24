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
        ShapeFileIO shapeFileIO = new();
        MapData mapData = shapeFileIO.Open("test1/BLKPOL1000YO.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;

        foreach (var mapDataObject in mapData)
        {
            Console.WriteLine(String.Format("WM-Object key: {0}, vertices count {1}, {2}", mapDataObject.Key, mapDataObject.Value.Count, mapData.HasDuplicatedPoints(mapDataObject.Key)));
        }

        WaterMark waterMark = new(
            new List<WaterMarkItem>()
            {
                new(1, 1, 1),
                new(1, 1, 2),
                new(1, 1, 3),
                new(0, 1, 4),

                new(1, 2, 1),
                new(1, 2, 2),
                new(0, 2, 3),
                new(0, 2, 4),

                new(0, 3, 1),
                new(1, 3, 2),
                new(1, 3, 3),
                new(1, 3, 4),

                new(1, 4, 1),
                new(0, 4, 2),
                new(0, 4, 3),
                new(0, 4, 4)
            },
            4
        );

        Console.WriteLine("-------------------");

        PrintWaterMark(waterMark);

        Console.WriteLine("-------------------");

        // waterMark.NextIterateArnlodTransform();

        MapDataProcessor mapDataProcessor = new(waterMark);

        var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);

        foreach (var mapDataObject in mapDataWithWaterMark)
        {
            Console.WriteLine(String.Format("WM-Object key: {0}, vertices count {1}, {2}", mapDataObject.Key, mapDataObject.Value.Count, mapDataWithWaterMark.HasDuplicatedPoints(mapDataObject.Key)));
        }

        Console.WriteLine("-------------------");

        var extractedWM = mapDataProcessor.WaterMarkExtracting(mapData);
        PrintWaterMark(extractedWM);
    }
}

