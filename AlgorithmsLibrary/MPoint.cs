using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class MPoint
    {
        public double X { set; get; }
        public double Y { set; get; }
        public double DistanceToVertex(MPoint v)
        {
            return Math.Sqrt(Math.Pow(X - v.X, 2) + Math.Pow(Y - v.Y, 2));
        }
        public MPoint(MPoint other)
        {
            this.X = other.X;
            this.Y = other.Y;
        }
        public MPoint() : this(0, 0) { }
        public MPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
