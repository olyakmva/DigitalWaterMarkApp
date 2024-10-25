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

    public static int[] getBinaryFromInteger(int length, int toBase, int number) {
        var source = Convert.ToString(number, toBase).PadLeft(length, '0');
        return source.Select(c => c - '0').ToArray();
    }

    public static WaterMark getWaterMark(int size, int number) {

        int[] binary = getBinaryFromInteger(size * size, 2, number);
        List<WaterMarkItem> items = new List<WaterMarkItem>();

        int currentItem = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++) {
                items.Add(new WaterMarkItem(binary[currentItem], i, j));
                currentItem++;
            }
        }

        return new WaterMark(items, size);
    }

    public static void Main()
    {
        /**
        * rivers1000k.shp
        * rlhlin1000.shp
        */

        ShapeFileIO shapeFileIO = new();
        MapData mapData = shapeFileIO.Open("test1/rlhlin1000.shp");
        List<KeyValuePair<int, List<MapPoint>>> objectList = mapData.MapObjDictionary;

        // foreach (var mapDataObject in mapData)
        // {
        //     Console.WriteLine(String.Format("WM-Object key: {0}, vertices count {1}, {2}", mapDataObject.Key, mapDataObject.Value.Count, mapData.HasDuplicatedPoints(mapDataObject.Key)));
        // }

        for (int i = 0; i < 16; i++)
        {
            WaterMark waterMark = getWaterMark(2, i);
            PrintWaterMark(waterMark);

            MapDataProcessor mapDataProcessor = new(waterMark);
            var mapDataWithWaterMark = mapDataProcessor.WaterMarkEmbedding(mapData);
            var extractedWM = mapDataProcessor.WaterMarkExtracting(mapDataWithWaterMark);
            PrintWaterMark(extractedWM);

            Console.WriteLine("-------------------");
        }
    }
}

