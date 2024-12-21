using DotSpatial.Data;
using SupportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DigitalWaterMarkApp {
    public static class MapProcessor {

        public static List<string> FindDirectoriesWithShpFiles(string directoryPath) {
            List<string> directoriesWithShp = new();

            try {
                if (Directory.Exists(directoryPath)) {
                    string[] subdirectories = Directory.GetDirectories(directoryPath);

                    foreach (string subdirectory in subdirectories) {
                        string[] files = Directory.GetFiles(subdirectory);
                        bool hasShp = Array.Exists(files, file => file.EndsWith(".shp", StringComparison.OrdinalIgnoreCase));
                        if (hasShp) {
                            directoriesWithShp.Add(subdirectory);
                        }

                        directoriesWithShp.AddRange(FindDirectoriesWithShpFiles(subdirectory));
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
            }

            return directoriesWithShp;
        }

        public static string GetFileNameWithoutExtension(string filePath) {
            int lastDotIndex = filePath.LastIndexOf('.');
            if (lastDotIndex > 0) {
                return filePath.Substring(filePath.LastIndexOf('\\') + 1, lastDotIndex - filePath.LastIndexOf('\\') - 1);
            }

            return filePath;
        }

        public static Map AnalyzeFolder(string folder) {
            Map map = new();
            string[] shapeFileNames = Directory.GetFiles(folder, "*.shp");
            foreach (string shapeFileName in shapeFileNames) {
                var fileName = GetFileNameWithoutExtension(shapeFileName);
                IFeatureSet inputShape = FeatureSet.Open(shapeFileName);
                var mapData = Converter.ToMapData(inputShape, fileName);
                if (mapData != null) {
                    map.Add(mapData);
                }
            }
            return map;
        }

        public static Map SecretCodeEmbedding(Map map, string secretCode) {
            Map embedding = map.Clone();

            WaterMark waterMark = WaterMark.ConvertToWaterMark(secretCode);
            MapDataProcessor mapDataProcessor = new(waterMark);
            foreach (MapData layer in embedding.MapLayers) {
                mapDataProcessor.WaterMarkEmbeddingViaLoopingDuplicateOfPoints(layer);
            }

            return embedding;
        }

        private static string FindCommonWord(List<(int objectsDiversity, string secret)> words) {

            words.Sort((s1, s2) => s2.objectsDiversity - s1.objectsDiversity);
            HashSet<string> zeroMatches = new();
            for (int i = 0; i < words.Count - 1; i++) {
                for (int j = i + 1; j < words.Count; j++) {
                    var distance = HammingDistance(words[i].secret, words[j].secret);
                    if (distance == 0) {
                        zeroMatches.Add(words[j].secret);
                    }
                }
            }

            return zeroMatches.Count != 0 ? zeroMatches.First() : words[0].secret;
        }

        private static int HammingDistance(string s1, string s2) {
            if (s1.Length != s2.Length) {
                return -1;
            }

            int distance = 0;
            for (int i = 0; i < s1.Length; i++) {
                if (s1[i] != s2[i]) {
                    distance++;
                }
            }

            return distance >= 1 ? 1 : 0;
        }

        public static string SecretCodeExtracting(Map map) {
            Map embedding = map.Clone();

            List<(int objectsDiversity, string secret)> secrets = new();
            foreach (MapData layer in map.MapLayers) {
                var secret = MapDataProcessor.FindWMDecimalFromLoopingsInMapData(layer).ToSecretCode();
                var objectsDiversity = layer.MapObjDictionary.Select(mapObject => mapObject.Value.Count).Distinct().Count();
                secrets.Add((objectsDiversity, secret));
            }

            return FindCommonWord(secrets);
        }
    }
}
