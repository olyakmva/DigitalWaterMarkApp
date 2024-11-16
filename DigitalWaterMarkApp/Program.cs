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

        MapData mapData = ShapeFileIO.Open("../Data/DataForDescriptor/Init16K/hdrLine1000i.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;
        Console.WriteLine(string.Format("Objects count in map: {0}", objectList.Count));

        long test = 69;

        Console.WriteLine("-------- ↓ ORIGINAL ↓ -----------");
        WaterMark waterMark = WaterMark.ConvertToWaterMark(test);
        PrintWaterMark(waterMark);

        MapDataProcessor mapDataProcessor = new(waterMark);
        mapDataProcessor.WaterMarkEmbeddingViaLoopingDuplicateOfPoints(mapData);

        // ShapeFileIO.Save("test1/rivers1000k_NEW.shp", mapData);

        Console.WriteLine("-------- ↓ FROM LOOPING ↓ -----------");
        var waterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(mapData);
        PrintWaterMark(waterMarkFromLooping);

        Console.WriteLine("-------- ↓ FROM 70% ↓ -----------");
        var per70_OfMapData = AttackRepropducer.DropRandomPercentageOfData(0.3, mapData);
        Console.WriteLine(string.Format("Objects count in map: {0}", per70_OfMapData.ObjectsCount));
        var per70_WaterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(per70_OfMapData);
        PrintWaterMark(per70_WaterMarkFromLooping);

        Console.WriteLine("-------- ↓ FROM 50% ↓ -----------");
        var per50_OfMapData = AttackRepropducer.DropRandomPercentageOfData(0.5, mapData);
        Console.WriteLine(string.Format("Objects count in map: {0}", per50_OfMapData.ObjectsCount));
        var per50_WaterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(per50_OfMapData);
        PrintWaterMark(per50_WaterMarkFromLooping);

        Console.WriteLine("-------- ↓ FROM 30% ↓ -----------");
        var per30_OfMapData = AttackRepropducer.DropRandomPercentageOfData(0.7, mapData);
        Console.WriteLine(string.Format("Objects count in map: {0}", per30_OfMapData.ObjectsCount));
        var per30_WaterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(per30_OfMapData);
        PrintWaterMark(per30_WaterMarkFromLooping);

        Console.WriteLine("-------- ↓ FROM 10% ↓ -----------");
        var per10_OfMapData = AttackRepropducer.DropRandomPercentageOfData(0.9, mapData);
        Console.WriteLine(string.Format("Objects count in map: {0}", per10_OfMapData.ObjectsCount));
        var per10_WaterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(per10_OfMapData);
        PrintWaterMark(per10_WaterMarkFromLooping);

        Console.WriteLine("-------- ↓ FROM 5% ↓ -----------");
        var per5_OfMapData = AttackRepropducer.DropRandomPercentageOfData(0.95, mapData);
        Console.WriteLine(string.Format("Objects count in map: {0}", per5_OfMapData.ObjectsCount));
        var per5_WaterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(per5_OfMapData);
        PrintWaterMark(per5_WaterMarkFromLooping);

        // Console.WriteLine("-------- ↓ FROM EXTRACTED ↓ -----------");

        // var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
        // var extractedWM = MapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark, waterMark.Length);
        // PrintWaterMark(extractedWM);
    }
}
