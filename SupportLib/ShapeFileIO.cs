using DotSpatial.Data;

namespace SupportLib
{
    public class ShapeFileIO
    {
        public static MapData Open(string shapeFileName)
        {
           var  _inputShape = FeatureSet.Open(shapeFileName);
            var mapData = Converter.ToMapData(_inputShape);
            mapData.FileName = shapeFileName.Remove(shapeFileName.Length-4);
            return mapData;
        }

        public static void Save(string fileName, MapData mapData)
        {
            IFeatureSet fs = Converter.ToShape(mapData);
            fs.SaveAs(fileName + ".shp", true);

        }
    }
}
