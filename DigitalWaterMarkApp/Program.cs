// Для пакетной обработки
//using DotSpatial.Data;
using SupportLib;

class Program
{
    public static void Main()
    {
        ShapeFileIO shapeFileIO = new();
        MapData mapData = shapeFileIO.Open("test1/BLKPOL1000YO.shp");
        var t = "";
    }
}

