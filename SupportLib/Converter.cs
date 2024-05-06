using DotSpatial.Data;
using NetTopologySuite.Geometries;

namespace SupportLib
{
    public static class Converter
    {
        public static MapData? ToMapData(IFeatureSet fSet)
        {
            var list = fSet.Features;
            if (list.Count == 0)
                return null;
            GeometryType type = GeometryType.Unspecified;

            switch (list[0].FeatureType)
            {
                case FeatureType.Line:
                    type = GeometryType.Line;
                    break;
                case FeatureType.Point:
                    type = GeometryType.Point;
                    break;
                case FeatureType.Polygon:
                    type = GeometryType.Polygon;
                    break;
                case FeatureType.MultiPoint:
                    type = GeometryType.MultiPoint;
                    break;
                case FeatureType.Unspecified:
                    break;
                default:
                    throw new ArgumentException("Unknown feature type");
            }

            var map = new MapData
            {
                Geometry = type
            };
            foreach (var item in list)
            {
                var shape = item.ToShape();
                int fid = 0;
                if (item.FeatureType == FeatureType.Polygon)
                {
                    fid = item.Fid;
                }
                else
                {
                    fid = Convert.ToInt32(shape.Attributes[0]);
                }
                var points = new List<MapPoint>();
                for(var t=0; t< shape.Vertices.Length;t+=2)
                {
                    var p = new MapPoint(shape.Vertices[t], shape.Vertices[t+1], fid, 1.0);
                    points.Add(p);
                }
                map.MapObjDictionary.Add(new KeyValuePair<int, List<MapPoint>>(fid, points));
            }
            return map;
        }

        public static IFeatureSet ToShape(MapData map)
        {
            FeatureType featureType = FeatureType.Unspecified;
            switch (map.Geometry)
            {
                case GeometryType.Line:
                    featureType = FeatureType.Line;
                    break;
                case GeometryType.Point:
                    featureType = FeatureType.Point;
                    break;
                case GeometryType.Polygon:
                    featureType = FeatureType.Polygon;
                    break;
                case GeometryType.MultiPoint:
                    featureType = FeatureType.MultiPoint;
                    break;
            }
            FeatureSet fs = new(featureType);
            foreach (var pairList in map.MapObjDictionary)
            {
                Coordinate[] coord = new Coordinate[pairList.Value.Count];
                for (int i = 0; i < pairList.Value.Count; i++)
                {
                    coord[i] = new Coordinate(pairList.Value[i].X, pairList.Value[i].Y);
                }
                var f = new Feature(featureType,coord);
                fs.Features.Add(f);
            }
            return fs;
        }
    }
}
