using AlgorithmsLibrary;
using SupportLib;

namespace MainForm.Controls
{
    public partial class AlgParamControl : UserControl
    {
        private ISimplificationAlgm? _algm;
        public event EventHandler CopyingParams;
        public MapData mapData;

        public bool IsPointReduction
        {
            get => rBtnPointReduct.Checked;
            set => rBtnPointReduct.Checked = value;
        }
        public bool IsParametr
        {
            get => rBtnParam.Checked;
            set => rBtnParam.Checked = value;
        }

        private double Tolerance
        {
            get => Convert.ToDouble(paramUpDown.Value);
            set => paramUpDown.Value = Convert.ToDecimal(value);
        }

        private double Percent
        {
            get => int.Parse(percentUpDown.Text);
            set => percentUpDown.Value = Convert.ToDecimal(value);
        }
        public string LayerName
        {
            set => lblName.Text = value;
            get => lblName.Text;
        }
        public bool IsChecked
        {
            get => checkRun.Checked;
            private set => checkRun.Checked = value;
        }
        public string AlgName
        {
            get { return AlgNameComboBox.Items[AlgNameComboBox.SelectedIndex].ToString(); }
            //set { AlgNameComboBox.SelectedIndex = value; }

        }
        public AlgParamControl()
        {
            InitializeComponent();
            AlgNameComboBox.SelectedIndex = 0;
        }
        public ISimplificationAlgm GetAlgorithm()
        {
            var p = new SimplificationAlgmParameters();
            _algm = AlgmFabrics.GetAlgmByNameAndParam(AlgName, IsPointReduction);
            p.Tolerance = Math.Truncate(Convert.ToDouble(paramUpDown.Value));
            p.RemainingPercent = double.Parse(percentUpDown.Text);
            _algm.Options = p;
            return _algm;
        }
        private void BtnCopyClick(object sender, EventArgs e)
        {
            CopyingParams?.Invoke(this, EventArgs.Empty);
        }
        public void OnCopyingParams(object sender, EventArgs e)
        {
            AlgParamControl ctrl = (AlgParamControl)sender;
            Tolerance = ctrl.Tolerance;
            Percent = ctrl.Percent;
            IsChecked = ctrl.IsChecked;
            IsParametr = ctrl.IsParametr;
            IsPointReduction = ctrl.IsPointReduction;

        }
    }
}
