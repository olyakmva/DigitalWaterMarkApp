using System.Diagnostics;
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

    public static String FormatWatchTime(Stopwatch sw) {
        TimeSpan ts = sw.Elapsed;
        return String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
           ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
    }
    public static string? EnsureDirectoryExists(string directoryPath) {
        try {
            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }
            return Path.GetFullPath(directoryPath);
        } catch (Exception ex) {
            Console.WriteLine($"Ошибка при создании директории: {ex.Message}");
            return null;
        }
    }

    public static void LogThis(string logging, StreamWriter sw) {
        Console.WriteLine(logging);
        sw.WriteLine(logging);
    }

    private static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

    public static string? FormatSize(string directoryPath) {
        long sizeInBytes;

        try {
            if (Directory.Exists(directoryPath)) {
                sizeInBytes = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories)
                                        .Sum(file => new FileInfo(file).Length);
            } else {
                return $"Ошибка: Директория '{directoryPath}' не найдена.";
            }
        } catch (Exception ex) {
            return $"Ошибка при вычислении размера директории: {ex.Message}";
        }

        if (sizeInBytes == 0) return "0 B"; // Обработка случая нулевого размера

        int i = 0;
        decimal size = sizeInBytes;
        while (size >= 1024 && i < SizeSuffixes.Length - 1) {
            size /= 1024;
            i++;
        }

        return $"{size:F2} {SizeSuffixes[i]}";
    }

    private static List<string> secrets = new() {
        "ЯРГУ", "ДИПЛОМ", "МАТЕМАТИКА", "ЗАЩИТА ДИПЛОМА", "КОМПЬЮТЕРНАЯ БЕЗОПАСНОСТЬ"
    };

    private static List<float> percentages = new() { 
        0.5F, 0.55F, 0.6F, 0.65F, 0.7F, 0.85F, 0.9F, 0.95F 
    };

    public static void Main() {

        List<string> analyzeFoldersWithShps = MapProcessor.FindDirectoriesWithShpFiles(".");
        foreach (var mapFolder in analyzeFoldersWithShps) {

            var resultFolder = EnsureDirectoryExists($"{mapFolder}_RESULTS");
            using StreamWriter sw = new($"{resultFolder}/log.txt");

            LogThis($"Processing map folder {mapFolder}...", sw);
            Stopwatch mapFolderProcessingWatch = Stopwatch.StartNew();

            Map map = MapProcessor.AnalyzeFolder(mapFolder);
            foreach (var secretCode in secrets) {
                LogThis($"Embedding '{secretCode}'...", sw);
                Stopwatch secretCodeEmbeddingWatch = Stopwatch.StartNew();
                Map mapWithSecretCode = MapProcessor.SecretCodeEmbedding(map, secretCode);
                secretCodeEmbeddingWatch.Stop();
                LogThis($"Embedding ended. Running time: {FormatWatchTime(secretCodeEmbeddingWatch)}", sw);

                LogThis($"Extracting after embedding...", sw);
                Stopwatch secretCodeExtractingWatch = Stopwatch.StartNew();
                string extractedSecret = MapProcessor.SecretCodeExtracting(mapWithSecretCode);
                secretCodeExtractingWatch.Stop();
                LogThis($"Extracting ended. Running time: {FormatWatchTime(secretCodeExtractingWatch)}", sw);
                LogThis($"Extracting result {extractedSecret}\n", sw);

                LogThis($"Begin running deletion attacks for secret '{secretCode}'...", sw);
                Stopwatch allDeletionAttacksWatch = Stopwatch.StartNew();
                foreach (var percentage in percentages) {
                    var deletionNormalizedPercentage = (int)Math.Round(percentage * 100, 2);
                    LogThis($"Begin deletion {deletionNormalizedPercentage}% vertices from {mapWithSecretCode.MapLayers.Count} layers...", sw);
                    Stopwatch deletionCurrentPercentageWatch = Stopwatch.StartNew();
                    Map mapCopy = mapWithSecretCode.Clone();
                    float meanPercentageDeletionFromEveryLayer = percentage / mapWithSecretCode.MapLayers.Count;
                    foreach (var mapLayer in mapWithSecretCode.MapLayers)
                        AttackRepropducer.DropRandomPercentageOfData(meanPercentageDeletionFromEveryLayer, mapLayer, true);
                    deletionCurrentPercentageWatch.Stop();
                    LogThis($"Deletion ended. Running time: {FormatWatchTime(deletionCurrentPercentageWatch)}", sw);

                    try {
                        LogThis("Saving map after attack...", sw);
                        foreach (var mapLayer in mapWithSecretCode.MapLayers)
                            ShapeFileIO.Save($"{resultFolder}/{mapLayer.FileName}_{deletionNormalizedPercentage}.shp", mapLayer);
                    } catch (Exception ex) {
                        LogThis($"Saving map after attack failed: {ex.Message}: {ex}", sw);
                    }

                    LogThis($"Count layers {mapCopy.MapLayers.Count}", sw);
                    LogThis($"Count objects {mapCopy.MapLayers.Sum(l => l.ObjectsCount)}", sw);
                    LogThis($"Count vertices {mapCopy.MapLayers.Sum(l => l.GetAllVertices().Count)}", sw);
                    LogThis($"Map size {FormatSize(resultFolder)}", sw);

                    try {
                        LogThis($"Extracting after deletion {deletionNormalizedPercentage}% vertices...", sw);
                        Stopwatch secretCodeExtractingAfterDeletionWatch = Stopwatch.StartNew();
                        string extractedSecretAfterDeletion = MapProcessor.SecretCodeExtracting(mapCopy);
                        secretCodeExtractingAfterDeletionWatch.Stop();
                        LogThis($"Extracting after deletion ended. Running time: {FormatWatchTime(secretCodeExtractingAfterDeletionWatch)}", sw);
                        LogThis($"Extracting result '{extractedSecretAfterDeletion}'", sw);
                    } catch (Exception ex) {
                        LogThis($"Extracting after deletion failed: {ex.Message}: {ex}", sw);
                    }
                    LogThis("\n", sw);
                }
                allDeletionAttacksWatch.Stop();
                LogThis($"All running deletion attacks for secret '{secretCode}' ended. Running time: {FormatWatchTime(allDeletionAttacksWatch)}", sw);
                LogThis("________________________________________", sw);
            }

            mapFolderProcessingWatch.Stop();
            LogThis($"Processing folder ended. Processing time: {FormatWatchTime(mapFolderProcessingWatch)}", sw);
            LogThis("========================================", sw);
        }

        Console.ReadKey();
    }
}
