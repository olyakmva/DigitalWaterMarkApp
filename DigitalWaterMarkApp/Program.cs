using System.Linq;
// Для пакетной обработки
//using DotSpatial.Data;
using DigitalWaterMarkApp;
using SupportLib;

class Program {
    public static void PrintWaterMark(WaterMark waterMark) {
        for (int i = 0; i < waterMark.Size * waterMark.Size; i++)
        {
            Console.Write(waterMark[i] + " ");
        }
        Console.WriteLine();
    }

    public static void Main() {

        MapData mapData = ShapeFileIO.Open("../Data/DataForDescriptor/Plus75/piplin1000.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;
        Console.WriteLine(string.Format("Objects count in map: {0}", objectList.Count));

        long test = 26593;

        Console.WriteLine("-------- ↓ ORIGINAL ↓ -----------");
        WaterMark waterMark = WaterMark.ConvertToWaterMark(test);
        PrintWaterMark(waterMark);

        MapDataProcessor mapDataProcessor = new(waterMark);
        mapDataProcessor.WaterMarkEmbeddingViaLoopingDuplicateOfPoints(mapData);

        // ShapeFileIO.Save("test1/rivers1000k_NEW.shp", mapData);

        Console.WriteLine("-------- ↓ FROM LOOPING ↓ -----------");

        var waterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(mapData);
        PrintWaterMark(waterMarkFromLooping);

        // Console.WriteLine("-------- ↓ FROM EXTRACTED ↓ -----------");

        // var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
        // var extractedWM = MapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark, waterMark.Length);
        // PrintWaterMark(extractedWM);
    }
}

