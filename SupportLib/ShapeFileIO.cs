using DotSpatial.Data;

namespace SupportLib
{
    public class ShapeFileIO
    {
        public MapData Open(string shapeFileName)
        {
           var  _inputShape = FeatureSet.Open(shapeFileName);
            var mapData = Converter.ToMapData(_inputShape);
            mapData.FileName = shapeFileName.Remove(shapeFileName.Length-4);
            return mapData;
        }

        public void Save(string fileName, MapData mapData)
        {
            IFeatureSet fs = Converter.ToShape(mapData);
            fs.SaveAs(fileName + ".shp", true);
            
        }
    }
}
