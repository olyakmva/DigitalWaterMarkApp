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

    public static List<string> ScanFolder(string directory) {
        var extensions = new List<string> { ".shp" };
        return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories)
            .Where(f => extensions
            .Any(extn => string.Compare(Path.GetExtension(f), extn, StringComparison.InvariantCultureIgnoreCase) == 0))
            .ToList();
    }

    public static void Main() {

        MapData mapData = ShapeFileIO.Open("C:\\Users\\Heimerdinger\\Documents\\Repositories\\DigitalWaterMarkApp\\Data\\DataForDescriptor\\Init16K\\hdrLine1000i.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;
        Console.WriteLine(string.Format("Число объектов: {0}", objectList.Count));

        string test = "HEIMER"; 

        Console.WriteLine("-------- ↓ Исходный ЦВЗ ↓ -----------");
        WaterMark waterMark = WaterMark.ConvertToWaterMark(test);
        PrintWaterMark(waterMark);

        MapDataProcessor mapDataProcessor = new(waterMark);
        mapDataProcessor.WaterMarkEmbeddingViaLoopingDuplicateOfPoints(mapData);

        Console.WriteLine("-------- ↓ Извлечение после встраивания ↓ -----------");
        var waterMarkFromLooping = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(mapData);
        PrintWaterMark(waterMarkFromLooping);
        Console.WriteLine(waterMarkFromLooping.ToSecretCode());

        Console.WriteLine("-------- ↓ Случайное удаление объектов ↓ -----------");
        List<float> percentages = new() { 0.3F, 0.5F, 0.7F, 0.85F, 0.9F, 0.95F };
        foreach (var percentage in percentages) {
            Console.WriteLine(string.Format("-------- ↓ Процент удаления {0}% ↓ -----------", (int) (percentage * 100)));
            var percentOfMapData = AttackRepropducer.DropRandomPercentageOfData(percentage, mapData);
            Console.WriteLine(string.Format("Число объектов после удаления: {0}", percentOfMapData.ObjectsCount));
            var waterMarkFromLoopingInPercentMapData = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(percentOfMapData);
            PrintWaterMark(waterMarkFromLoopingInPercentMapData);
            Console.WriteLine(waterMarkFromLoopingInPercentMapData.ToSecretCode());
        }

        Console.WriteLine("-------- ↓ Случайное перемешивание ↓ -----------");
        var shufflingAttackResult = AttackRepropducer.ShuffleObjectsInMap(mapData);
        Console.WriteLine(string.Format("Similiar objects percentage: {0}", shufflingAttackResult.similiarObjectsPercentage));
        var waterMarkFromLoopingInShufflingMapData = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(shufflingAttackResult.shufflingMapData);
        PrintWaterMark(waterMarkFromLoopingInShufflingMapData);
        Console.WriteLine(waterMarkFromLoopingInShufflingMapData.ToSecretCode());

        Console.WriteLine("-------- ↓ Извлечение после встраивания через хэш-функцию ↓ -----------");

        var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
        var extractedWM = MapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark, waterMark.Length);
        Console.WriteLine(string.Join(' ', extractedWM.Select(i => i.ToString())));

        Console.ReadKey();
    }
}
