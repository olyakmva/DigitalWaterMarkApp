using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

namespace SupportLib
{
    // Данные слоя карты. В списке хранятся точки на карте
    // слой одной геометрии
    [Serializable]
    public class MapData : IEnumerable<KeyValuePair<int, List<MapPoint>>>
    {
        public List<KeyValuePair<int, List<MapPoint>>> MapObjDictionary { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int Count => GetAllVertices().Count;
        public string ColorName { get;  set; }
        public GeometryType Geometry {  get; set; }

        public MapData()
        {
            MapObjDictionary = new List<KeyValuePair<int, List<MapPoint>>>();
            ColorName = Colors.GetNext();
        }

        public MapData(GeometryType type) :this()
        {
            Geometry = type;
        }

        public List<MapPoint> GetAllVertices()
        {
            var resultList = new List<MapPoint>();
            foreach (var objPair in MapObjDictionary)
            {
                resultList.AddRange(objPair.Value);
            }
            return resultList;
        }

        public MapData Clone()
        {
            MapData clone;
            var bf = new BinaryFormatter();
            using (Stream fs = new FileStream("temp.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                bf.Serialize(fs, this);
            }
            using (Stream fs = new FileStream("temp.bin", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                clone = (MapData)bf.Deserialize(fs);
            }
            return clone;
        }

        public void ClearWeights()
        {
            foreach (var chain in MapObjDictionary)
            {
                foreach (var vertex in chain.Value)
                {
                    vertex.Weight = 1;
                }
            }
        }

        public MapData MultiplyOffsetMapData(double offset = 0, double mul_offset = 1)
        {
            var result = new MapData();
            result.FileName = FileName + "_Clone";
            result.ColorName = ColorName;
            result.Geometry = Geometry;

            foreach (var obj in MapObjDictionary)
            {
                var tmp = new List<MapPoint>();

                foreach (var point in obj.Value)
                {
                    tmp.Add(new MapPoint(point.X, point.Y, point.Id, point.Weight));
                }

                foreach (var t in tmp)
                {
                    t.X = t.X * mul_offset + offset;
                    t.Y = t.Y * mul_offset + offset;
                }
                result.MapObjDictionary.Add(new KeyValuePair<int, List<MapPoint>>(obj.Key, tmp));
            }
            return result;
        }

        public List<MapPoint> this[int index]
        {
            get => this.MapObjDictionary[index].Value;
            private set { }
        }

        public void SwapMapObjects(int firstMapObjectIndex, int secondMapObjectIndex) {
            (this.MapObjDictionary[secondMapObjectIndex], this.MapObjDictionary[firstMapObjectIndex])
                = (this.MapObjDictionary[firstMapObjectIndex], this.MapObjDictionary[secondMapObjectIndex]);
        }

        public void DuplicatePointInObjectByIndexAtPosition(int objectIndex, int waterMarkEmbeddingItemIdx) {
            var mapObject = this.MapObjDictionary[objectIndex];
            var mapObjectPoints = mapObject.Value;
            var duplicatingPosition = waterMarkEmbeddingItemIdx % mapObjectPoints.Count;
            mapObjectPoints.Insert(duplicatingPosition, mapObjectPoints[duplicatingPosition]);
        }

        public Object HasDuplicatedPoints(int objectIndex) {
            var duplicates = this.MapObjDictionary[objectIndex - 1].Value
                .GroupBy(x => new { x.X, x.Y })
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if (this.Geometry.Equals(GeometryType.Polygon)) {
                return duplicates.Count > 1;
            }

            return duplicates.Count > 0;
        }

        public IEnumerator<KeyValuePair<int, List<MapPoint>>> GetEnumerator()
        {
            foreach (KeyValuePair<int, List<MapPoint>> mapObject in MapObjDictionary)
            {
                yield return mapObject;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
