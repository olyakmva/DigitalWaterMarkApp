using SupportLib;


namespace AlgorithmsLibrary
{
    public static class HausdorfDistance
    {
        public static double Get(List<MapPoint> points1, List<MapPoint> points2)
        {
            double max = 0;
            if (points1.Count == 0 || points2.Count == 0)
                throw new ArgumentException("Empty points list");
            if (points1.Count == points2.Count)
            {
                for (int j = 0; j < points1.Count; j++)
                {
                    max = Math.Max(max, points1[j].DistanceToVertex(points2[j]));
                }
            }
            else
            {
                max = Math.Max(Max(points1, points2), Max(points2, points1));
            }
            return Math.Round(max,2);
        }

        private static double Max(List<MapPoint> lst1, List<MapPoint> lst2)
        {
            double max = 0;
            for (int j = 1; j < (lst1.Count - 1); j++)
            {
                double min = double.MaxValue;
                for (int k = 0; k < (lst2.Count - 1); k++)
                {
                    double d;
                    if (lst2[k].Equals(lst2[k + 1]))
                    {
                        d = lst2[k].DistanceToVertex(lst1[j]);
                    }
                    else
                    {
                        var line = new Line(lst2[k], lst2[k + 1]);
                        d = line.GetDistance(lst1[j]);
                    }
                    if (d < min)
                        min = d;
                    if (min < double.Epsilon)
                        break;
                }
                if (min > max)
                    max = min;
            }
            return max;
        }
    }

}

