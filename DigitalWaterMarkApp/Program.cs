using System.Linq;
// Для пакетной обработки
//using DotSpatial.Data;
using DigitalWaterMarkApp;
using SupportLib;

class Program {
    public static void PrintWaterMark(WaterMark waterMark) {
        for (int i = 0; i < waterMark.Size * waterMark.Size; i++) {
            Console.Write(waterMark[i] + " ");
        }
        Console.WriteLine();
    }

    public static void Main() {

        string pathToMapFolder = "C:\\Users\\Heimerdinger\\Documents\\Repositories\\DigitalWaterMarkApp\\Data\\DataForDescriptor\\Plus75";

        string secretCode = "DIPLOM";

        Map map = MapProcessor.AnalyzeFolder(pathToMapFolder);
        Map mapWithSecretCode = MapProcessor.SecretCodeEmbedding(map, secretCode);
        string extractedSecret = MapProcessor.SecretCodeExtracting(mapWithSecretCode);
        Console.WriteLine("-------- ↓ Извлечение после встраивания ↓ -----------");
        Console.WriteLine(extractedSecret);

        //Console.WriteLine("-------- ↓ Случайное удаление объектов ↓ -----------");
        //List<float> percentages = new() { 0.5F, 0.55F, 0.6F, 0.65F, 0.7F, 0.85F, 0.9F, 0.95F };
        //foreach (var percentage in percentages) {
        //    Console.WriteLine(string.Format("-------- ↓ Процент удаления {0}% ↓ -----------", (int) (percentage * 100)));
        //    var percentOfMapData = AttackRepropducer.DropRandomPercentageOfData(percentage, mapData);
        //    Console.WriteLine(string.Format("Число объектов после удаления: {0}", percentOfMapData.ObjectsCount));
        //    var waterMarkFromLoopingInPercentMapData = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(percentOfMapData);
        //    PrintWaterMark(waterMarkFromLoopingInPercentMapData);
        //    Console.WriteLine(waterMarkFromLoopingInPercentMapData.ToSecretCode());
        //}

        //Console.WriteLine("-------- ↓ Случайное перемешивание ↓ -----------");
        //var shufflingAttackResult = AttackRepropducer.ShuffleObjectsInMap(mapData);
        //Console.WriteLine(string.Format("Similiar objects percentage: {0}", shufflingAttackResult.similiarObjectsPercentage));
        //var waterMarkFromLoopingInShufflingMapData = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(shufflingAttackResult.shufflingMapData);
        //PrintWaterMark(waterMarkFromLoopingInShufflingMapData);
        //Console.WriteLine(waterMarkFromLoopingInShufflingMapData.ToSecretCode());

        //Console.WriteLine("-------- ↓ Извлечение после встраивания через хэш-функцию ↓ -----------");

        //var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
        //var extractedWM = MapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark, waterMark.Length);
        //Console.WriteLine(string.Join(' ', extractedWM.Select(i => i.ToString())));

        Console.ReadKey();
    }
}
