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

        MapData mapData = ShapeFileIO.Open("C:\\Users\\Heimerdinger\\Documents\\Repositories\\DigitalWaterMarkApp\\Data\\DataForDescriptor\\Init16K\\hdrLine1000i.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;
        Console.WriteLine(string.Format("Objects count in map: {0}", objectList.Count));

        long test = 69;

        Console.WriteLine("-------- ↓ ORIGINAL ↓ -----------");
        WaterMark waterMark = WaterMark.ConvertToWaterMark(test);
        PrintWaterMark(waterMark);

        MapDataProcessor mapDataProcessor = new(waterMark);
        mapDataProcessor.WaterMarkEmbeddingViaLoopingDuplicateOfPoints(mapData);

        //ShapeFileIO.Save("../Data/DataForDescriptor/Init16K/hdrLine1000i_new.shp", mapData);

        Console.WriteLine("-------- ↓ FROM LOOPING ↓ -----------");
        var waterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(mapData);
        PrintWaterMark(waterMarkFromLooping);

        Console.WriteLine("-------- ↓ TESTING DROP ATTACK ↓ -----------");
        List<float> percentages = new() { 0.3F, 0.5F, 0.7F, 0.8F, 0.9F, 0.95F };
        foreach (var percentage in percentages) {
            Console.WriteLine(string.Format("-------- ↓ FROM {0}% ↓ -----------", (int) ((1 - percentage) * 100)));
            var percentOfMapData = AttackRepropducer.DropRandomPercentageOfData(percentage, mapData);
            Console.WriteLine(string.Format("Objects count in map: {0}", percentOfMapData.ObjectsCount));
            var waterMarkFromLoopingInPercentMapData = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(percentOfMapData);
            PrintWaterMark(waterMarkFromLoopingInPercentMapData);
        }

        Console.WriteLine("-------- ↓ TESTING SHUFFLE ATTACK ↓ -----------");
        var shufflingAttackResult = AttackRepropducer.ShuffleObjectsInMap(mapData);
        Console.WriteLine(string.Format("Similiar objects percentage: {0}", shufflingAttackResult.similiarObjectsPercentage));
        var waterMarkFromLoopingInShufflingMapData = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(shufflingAttackResult.shufflingMapData);
        PrintWaterMark(waterMarkFromLoopingInShufflingMapData);

        Console.WriteLine("-------- ↓ FROM EXTRACTED ↓ -----------");

        var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
        var extractedWM = MapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark, waterMark.Length);
        Console.WriteLine(string.Join(' ', extractedWM.Select(i => i.ToString())));
    }
}
