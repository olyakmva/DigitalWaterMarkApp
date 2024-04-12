
namespace SupportLib
{
    public static class Colors
    {
        private static int _index;

        private static readonly List<string> ColorList = new List<string>(new[]
        {
            "RoyalBlue","Red", "DarkGreen", "DarkViolet", "LightSkyBlue",
            "Orange", "ForestGreen", "Pink", "SandyBrown","Black"
        });
        public static string GetNext()
        {
            var result = ColorList[_index];
            _index = (_index + 1) % ColorList.Count;
            return result;
        }
        public static void Init()
        {
            _index = 0;
        }
    }
}
