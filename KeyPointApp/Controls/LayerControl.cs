using DotSpatial.Data;
using SupportLib;

namespace MainForm.Controls
{
    public partial class LayerControl : UserControl
    {
        private Layer MapLayer { get; }
        public event EventHandler CheckedChanged;

        public LayerControl(Layer l)
        {
            InitializeComponent();
            MapLayer = l;
            layerCheckBox.Checked = true;
            layerCheckBox.Text = l.AlgorithmName.PadRight(20);
            lblPoints.Text = "n=" + l.MapData.Count.ToString();
        }
        private void LayerCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            MapLayer.Visible = layerCheckBox.Checked;
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string fileName = MapLayer.AlgorithmName + MapLayer.MapData.FileName + ".shp";
            string outFolder = @"Output";
            if (!Directory.Exists(outFolder))
            {
                Directory.CreateDirectory(outFolder);
            }
            var fileNameWithPath = Path.Combine(outFolder, fileName);
            IFeatureSet fs = Converter.ToShape(MapLayer.MapData);
            fs.SaveAs(fileNameWithPath, true);
        }
    }
}
