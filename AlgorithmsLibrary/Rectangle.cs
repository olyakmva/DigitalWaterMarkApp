using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsLibrary
{
    public class Rectangle
    {
        #region Fiels
        /// <summary>
        /// LowLeft, UpLeft, UpRight, LowRight points of Rectangle
        /// </summary>
        public MPoint LowLeft, UpLeft, UpRight, LowRight;

        /// <summary>
        /// Side length of Rectangle
        /// </summary>
        public double Side { get => UpLeft.DistanceToVertex(UpRight); }
        #endregion

        #region Constructors
        /// <summary>
        /// Contrustor of Rectangle with four Points
        /// </summary>
        /// <param name="A">LowLeft point</param>
        /// <param name="B">Upleft point</param>
        /// <param name="C">UpRight point</param>
        /// <param name="D">LowRight point</param>
        public Rectangle(MPoint A, MPoint B, MPoint C, MPoint D)
        {
            LowLeft = A;
            UpLeft = B;
            UpRight = C;
            LowRight = D;
        }

        /// <summary>
        /// Default constructor of Rectangle (without points)
        /// </summary>
        public Rectangle()
        {

        }

        /// <summary>
        /// Constructor of other Rectangle (copy of Rectangle)
        /// </summary>
        /// <param name="other">Other Rectangle</param>
        public Rectangle(Rectangle other)
        {
            LowLeft = other.LowLeft;
            UpLeft = other.UpLeft;
            UpRight = other.UpRight;
            LowRight = other.LowRight;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method answer the question : "Are Rectangles equals?"
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool RectangleEquals(Rectangle other)
        {
            return this.LowLeft.X == other.LowLeft.X &&
                   this.LowLeft.Y == other.LowRight.Y &&
                   this.UpLeft.X == other.UpLeft.X &&
                   this.UpLeft.Y == other.UpLeft.Y &&
                   this.UpRight.X == other.UpRight.X &&
                   this.UpRight.Y == other.UpRight.Y &&
                   this.LowLeft.X == other.LowLeft.X &&
                   this.LowLeft.Y == other.LowLeft.Y;

        }
        /// <summary>
        /// This method answer the question : "Is the point located in Rectangle?"
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsIn(MPoint point)
        {
            double epsilon = 1.0 / Math.Pow(10, 8);
            return (this.LowLeft.X < point.X &&
                   this.LowLeft.Y < point.Y &&
                   this.UpRight.X > point.X &&
                   this.UpRight.Y > point.Y)
                ||
                    (this.LowLeft.X < point.X &&
                    this.UpRight.X > point.X &&
                        (Math.Abs(this.LowLeft.Y - point.Y) < epsilon ||
                        Math.Abs(this.UpLeft.Y - point.Y) < epsilon))
                ||
                    (this.LowLeft.Y < point.Y &&
                    this.UpRight.Y > point.Y &&
                    (Math.Abs(this.LowLeft.X - point.X) < epsilon ||
                    Math.Abs(this.UpRight.X - point.X) < epsilon))
                ||
                    (Math.Abs(this.LowLeft.X - point.X) < epsilon &&
                    Math.Abs(this.LowLeft.Y - point.Y) < epsilon)
                ||
                    (Math.Abs(this.UpLeft.X - point.X) < epsilon &&
                    Math.Abs(this.UpLeft.Y - point.Y) < epsilon)
                ||
                    (Math.Abs(this.UpRight.X - point.X) < epsilon &&
                    Math.Abs(this.UpRight.Y - point.Y) < epsilon)
                ||
                    (Math.Abs(this.LowRight.X - point.X) < epsilon &&
                    Math.Abs(this.LowRight.Y - point.Y) < epsilon);
        }

        /// <summary>
        /// Method of receiving point of Rectangle
        /// </summary>
        /// <returns></returns>
        public List<MPoint> GetPoints()
        {
            return new List<MPoint>(new[] { LowLeft, UpLeft, UpRight, LowRight });
        }
        #endregion
    }
}
