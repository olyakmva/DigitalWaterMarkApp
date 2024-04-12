using System.Text;


namespace SupportLib
{
    public class Layer
    {
        public MapData MapData { get; private set; }
        public string Color { get; private set; }
        public string AlgorithmName { get; private set; }
        public bool Visible { get; set; }
               
        public Layer(MapData map, string algName = "input")
        {
            MapData = map;
            Color = Colors.GetNext();
            AlgorithmName = algName;
            Visible = true;
            
        }       
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(AlgorithmName);
            sb.Append(';');
            return sb.ToString();
        }

        public static string GetDescription()
        {
            string s = "AlgorithmName;OutScale;";
            return s;
        }
    }
}
