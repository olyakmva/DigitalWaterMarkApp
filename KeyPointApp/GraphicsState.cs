using SupportLib;

namespace KeyPointApp
{
    public class GraphicsState
    {
        public double DefscaleX { get; set; }
        public double DefscaleY { get; set; }
        public double DefCenterX { get; set; }
        public double DefCenferY { get; set; }
        public int Mousex { get; set; }
        public int Mousey { get; set; }
        public double CenterX { get; set; }//центр, относительно которого вводятся координаты
        public double CenterY { get; set; }
        public double Scale { get; set; }//Масштаб метр на пиксель
        public double PixelPerMm { get; set; }

        /// <summary>
        /// Перевод вершины в координаты для рисования
        /// </summary>
        /// <param name="v">Вершина</param>
        /// <param name="height">Высота PictureBox</param>
        /// <returns></returns>
        public Point GetPoint(MapPoint v, int height)
        {
            var x = (int)((v.X - CenterX) / Scale);
            var y = (int)((height - (v.Y - CenterY)) / Scale);
            return new Point(x, y);
        }
        /// <summary>
        /// Изменение масштаба
        /// </summary>
        /// <param name="z"></param>
        /// <param name="sizex">Ширина picturebox</param>
        /// <param name="sizey">Высота picturebox</param>
        public void Zoom(int z, int sizex, int sizey)
        {
            if (z > 0)
                Scale *= Math.Pow(2, z);
            double temp = sizex * z;
            CenterX -= temp / 4 * Scale;
            temp = sizey * z;
            CenterY += temp / 4 * Scale;
            if (z < 0)
                Scale *= Math.Pow(2, z);
        }

        public void InitG(Graphics g, int xy)
        {
            const double mmPerInch = 25.41;
            if (xy == 1)
                PixelPerMm = g.DpiX / mmPerInch;
            else PixelPerMm = g.DpiY / mmPerInch;
        }


    }
}
