using SupportLib;
using System.Runtime.Serialization;


namespace AlgorithmsLibrary
{
    public class Line
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Line(MapPoint v1, MapPoint v2)
        {
            A = v2.Y - v1.Y;
            B = (v1.X - v2.X);
            C = v1.Y * (-1) * B - v1.X * A;
            if (Math.Abs(A) < double.Epsilon && Math.Abs(B) < double.Epsilon && Math.Abs(C) < double.Epsilon)
            {
                throw new LineCoefEqualsZeroException(" коэффициенты уравнения прямой= нулю");
            }
        }

        public Line()
        { }

        public override string ToString()
        {
            return string.Format("{0:f2}x + {1:f2}y + {2:f2} =0", A, B, C);
        }

        public double GetDistance(MapPoint v)
        {
            return Math.Abs(A * v.X + B * v.Y + C) / Math.Sqrt(A * A + B * B);
        }

        public double GetAngle(Line otherLine)
        {

            const double tolerance = 0.001;

            if ((Math.Abs(A) < tolerance && Math.Abs(otherLine.A) < tolerance) ||
                (Math.Abs(B) < tolerance && Math.Abs(otherLine.B) < tolerance))
            {
                return 0;
            }
            if (Math.Abs(A / B - otherLine.A / otherLine.B) < tolerance)
            {
                return 0;
            }

            double scalar = A * otherLine.A + B * otherLine.B;
            if (Math.Abs(scalar) < tolerance)
            {
                return 90;
            }
            double tangens = (A * otherLine.B - otherLine.A * B) / scalar;

            return (Math.Atan(tangens) * 180) / Math.PI;
        }

        public double GetRadiansAngle(Line otherLine)
        {

            const double tolerance = 0.001;
            var tangens = GetAngleTangens(otherLine);
            if (Math.Abs(tangens - 1000) < tolerance)
                return 90;
            if (Math.Abs(tangens) < tolerance)
                return 0;
            return (Math.Atan(tangens));
        }

        public double GetGradusAngle(Line otherLine)
        {
            var tangens = GetAngleTangens(otherLine);
            const double tolerance = 0.001;
            if (Math.Abs(tangens - 1000) < tolerance)
                return 90;
            if (Math.Abs(tangens) < tolerance)
                return 0;
            return (Math.Atan(tangens) * 180) / Math.PI;
        }

        public double GetAngleTangens(Line otherLine)
        {

            const double tolerance = 0.001;

            if ((Math.Abs(A) < tolerance && Math.Abs(otherLine.A) < tolerance) ||
                (Math.Abs(B) < tolerance && Math.Abs(otherLine.B) < tolerance))
            {
                return 0;
            }
            if (Math.Abs(A / B - otherLine.A / otherLine.B) < tolerance)
            { // прямые параллельны - угол = 180  тангенс равен нулю
                return 0;
            }

            double scalar = A * otherLine.A + B * otherLine.B;
            if (Math.Abs(scalar) < tolerance)
            {
                return 1000; // бесконечность
            }
            double tangens = (A * otherLine.B - otherLine.A * B) / scalar;

            return tangens;
        }

        public MapPoint GetIntersectionPoint(Line otherLine)
        {
            var delta = A * otherLine.B - B * otherLine.A;
            if (Math.Abs(delta) < double.Epsilon)
                return null;
            var delta1 = (-1 * C * otherLine.B + B * otherLine.C);
            var delta2 = A * otherLine.C * (-1) + otherLine.A * C;
            return new MapPoint { X = delta1 / delta, Y = delta2 / delta };
        }

        public MapPoint GetPerpendicularFoundationPoint(MapPoint initVertex)
        {
            var result = new MapPoint();
            var delta = B * B + A * A;
            var delta1 = (B * initVertex.X - A * initVertex.Y) * B - C * A;
            var delta2 = B * C * (-1) - (B * initVertex.X - A * initVertex.Y) * A;
            result.X = delta1 / delta;
            result.Y = delta2 / delta;
            return result;
        }
        public int GetSign(MapPoint v)
        {
            double result = A * v.X + B * v.Y + C;
            const double tolerance = 0.001;
            if (Math.Abs(result) < tolerance)
                return 0;
            if (result > 0)
                return 1;
            return -1;
        }

        public double GetY(double x)
        {
            if (Math.Abs(B) > double.Epsilon)
                return (A * x + C) / (-1 * B);
            return 0;
        }
        public double GetX(double y)
        {
            if (Math.Abs(A) > double.Epsilon)
                return (B * y + C) / (-1 * A);
            return 0;
        }
        /// <summary>
        /// возвращает прямую перепендикулярную данной и проходящую через заданную точку 
        /// </summary>
        /// <param name="point"> точка</param>
        /// <returns></returns>
        public Line GetPerpendicularLine(MapPoint point)
        {
            var line = new Line
            {
                A = B,
                B = -1 * A,
                C = point.Y * A - point.X * B
            };
            return line;
        }
    }
    [Serializable]
    public class LineCoefEqualsZeroException : Exception
    {
        public LineCoefEqualsZeroException(string message) : base(message)
        { }

        protected LineCoefEqualsZeroException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        { }
    }
}
