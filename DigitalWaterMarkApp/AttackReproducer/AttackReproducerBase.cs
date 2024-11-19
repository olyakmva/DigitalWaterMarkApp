using SupportLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWaterMarkApp.AttackReproducer {
    public abstract class AttackReproducerBase : IAttackReproducer {

        public abstract MapData RunAttack(MapData mapData, Dictionary<string, object> parameters);

        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
        public static string FormatSize(long bytes) {
            int counter = 0;
            decimal number = bytes;
            while (Math.Round(number / 1024) >= 1) {
                number /= 1024;
                counter++;
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }

        public MapData Run(MapData mapData, Dictionary<string, object> parameters) {

            Console.WriteLine($"Запуск {GetType().Name}");

            ShapeFileIO.Save("temp", mapData);
            Console.WriteLine($"Исходный размер: {FormatSize(new FileInfo("temp.shp").Length)}");
            Console.WriteLine($"Исходное число объектов: {mapData.ObjectsCount}");

            var sw = Stopwatch.StartNew();
            MapData result = RunAttack(mapData, parameters);
            sw.Stop();

            ShapeFileIO.Save("temp", result);
            Console.WriteLine($"Размер после атаки: {FormatSize(new FileInfo("temp.shp").Length)}");
            Console.WriteLine($"Число объектов после атаки: {mapData.ObjectsCount}");
            Console.WriteLine($"Завершено. Время выполнения: {sw.ElapsedMilliseconds / 1000} сек");
            Console.WriteLine();

            return result;

        }
    }
}
