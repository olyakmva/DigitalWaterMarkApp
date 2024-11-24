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

        Console.WriteLine("-------- ↓ Случайное удаление объектов ↓ -----------");
        List<float> percentages = new() { 0.5F, 0.55F, 0.6F, 0.65F, 0.7F, 0.85F, 0.9F, 0.95F };
        foreach (var percentage in percentages) {
            Map mapCopy = mapWithSecretCode.Clone();
            Console.WriteLine(string.Format("-------- ↓ Процент удаления {0}% ↓ -----------", (int)(percentage * 100)));
            foreach (var mapLayer in mapWithSecretCode.MapLayers) {
                AttackRepropducer.DropRandomPercentageOfData(percentage, mapLayer, true);
                Console.WriteLine(string.Format("Число объектов после удаления в слое {0}: {1}", mapLayer.FileName, mapLayer.ObjectsCount));
            }

            try {
                Console.WriteLine(MapProcessor.SecretCodeExtracting(mapCopy));
            } catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }
        }

        Console.ReadKey();
    }
}
