namespace MainForm.Controls
{
    partial class AlgParamControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblName = new Label();
            lblTolerance = new Label();
            paramUpDown = new NumericUpDown();
            btnCopy = new Button();
            label4 = new Label();
            percentUpDown = new NumericUpDown();
            label5 = new Label();
            checkRun = new CheckBox();
            AlgNameComboBox = new ComboBox();
            lblAlgName = new Label();
            critGroupBox = new GroupBox();
            rBtnPointReduct = new RadioButton();
            rBtnParam = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)paramUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)percentUpDown).BeginInit();
            critGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            lblName.Location = new Point(54, 6);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(68, 25);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            // 
            // lblTolerance
            // 
            lblTolerance.AutoSize = true;
            lblTolerance.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            lblTolerance.Location = new Point(10, 88);
            lblTolerance.Margin = new Padding(4, 0, 4, 0);
            lblTolerance.Name = "lblTolerance";
            lblTolerance.Size = new Size(121, 25);
            lblTolerance.TabIndex = 3;
            lblTolerance.Text = "Alg. param:";
            // 
            // paramUpDown
            // 
            paramUpDown.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            paramUpDown.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            paramUpDown.Location = new Point(135, 82);
            paramUpDown.Margin = new Padding(4, 5, 4, 5);
            paramUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            paramUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            paramUpDown.Name = "paramUpDown";
            paramUpDown.Size = new Size(111, 28);
            paramUpDown.TabIndex = 4;
            paramUpDown.TextAlign = HorizontalAlignment.Center;
            paramUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(275, 0);
            btnCopy.Margin = new Padding(4, 5, 4, 5);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(36, 36);
            btnCopy.TabIndex = 5;
            btnCopy.Text = "C";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += BtnCopyClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(8, 129);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(123, 25);
            label4.TabIndex = 6;
            label4.Text = "PointRedct:";
            // 
            // percentUpDown
            // 
            percentUpDown.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            percentUpDown.Location = new Point(135, 130);
            percentUpDown.Margin = new Padding(4, 5, 4, 5);
            percentUpDown.Maximum = new decimal(new int[] { 95, 0, 0, 0 });
            percentUpDown.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            percentUpDown.Name = "percentUpDown";
            percentUpDown.Size = new Size(109, 28);
            percentUpDown.TabIndex = 7;
            percentUpDown.TextAlign = HorizontalAlignment.Center;
            percentUpDown.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(241, 136);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(23, 20);
            label5.TabIndex = 10;
            label5.Text = "%";
            // 
            // checkRun
            // 
            checkRun.AutoSize = true;
            checkRun.Checked = true;
            checkRun.CheckState = CheckState.Checked;
            checkRun.Location = new Point(8, 6);
            checkRun.Margin = new Padding(4, 5, 4, 5);
            checkRun.Name = "checkRun";
            checkRun.Size = new Size(22, 21);
            checkRun.TabIndex = 11;
            checkRun.UseVisualStyleBackColor = true;
            // 
            // AlgNameComboBox
            // 
            AlgNameComboBox.FormattingEnabled = true;
            AlgNameComboBox.Items.AddRange(new object[] { "SleeveFit", "DouglasPeucker", "VisvWhyatt" });
            AlgNameComboBox.Location = new Point(135, 39);
            AlgNameComboBox.Margin = new Padding(4, 4, 4, 4);
            AlgNameComboBox.Name = "AlgNameComboBox";
            AlgNameComboBox.Size = new Size(139, 33);
            AlgNameComboBox.TabIndex = 12;
            // 
            // lblAlgName
            // 
            lblAlgName.AutoSize = true;
            lblAlgName.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            lblAlgName.Location = new Point(8, 45);
            lblAlgName.Margin = new Padding(4, 0, 4, 0);
            lblAlgName.Name = "lblAlgName";
            lblAlgName.Size = new Size(105, 30);
            lblAlgName.TabIndex = 13;
            lblAlgName.Text = "AlgName:";
            // 
            // critGroupBox
            // 
            critGroupBox.Controls.Add(rBtnPointReduct);
            critGroupBox.Controls.Add(rBtnParam);
            critGroupBox.Location = new Point(259, 82);
            critGroupBox.Margin = new Padding(4, 4, 4, 4);
            critGroupBox.Name = "critGroupBox";
            critGroupBox.Padding = new Padding(4, 4, 4, 4);
            critGroupBox.Size = new Size(52, 89);
            critGroupBox.TabIndex = 14;
            critGroupBox.TabStop = false;
            // 
            // rBtnPointReduct
            // 
            rBtnPointReduct.AutoSize = true;
            rBtnPointReduct.Checked = true;
            rBtnPointReduct.Location = new Point(11, 51);
            rBtnPointReduct.Margin = new Padding(4, 4, 4, 4);
            rBtnPointReduct.Name = "rBtnPointReduct";
            rBtnPointReduct.Size = new Size(21, 20);
            rBtnPointReduct.TabIndex = 1;
            rBtnPointReduct.TabStop = true;
            rBtnPointReduct.UseVisualStyleBackColor = true;
            // 
            // rBtnParam
            // 
            rBtnParam.AutoSize = true;
            rBtnParam.Location = new Point(11, 5);
            rBtnParam.Margin = new Padding(4, 4, 4, 4);
            rBtnParam.Name = "rBtnParam";
            rBtnParam.Size = new Size(21, 20);
            rBtnParam.TabIndex = 0;
            rBtnParam.UseVisualStyleBackColor = true;
            // 
            // AlgParamControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightGray;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(critGroupBox);
            Controls.Add(lblAlgName);
            Controls.Add(AlgNameComboBox);
            Controls.Add(checkRun);
            Controls.Add(label5);
            Controls.Add(percentUpDown);
            Controls.Add(label4);
            Controls.Add(btnCopy);
            Controls.Add(paramUpDown);
            Controls.Add(lblTolerance);
            Controls.Add(lblName);
            Margin = new Padding(4, 5, 4, 5);
            Name = "AlgParamControl";
            Size = new Size(320, 163);
            ((System.ComponentModel.ISupportInitialize)paramUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)percentUpDown).EndInit();
            critGroupBox.ResumeLayout(false);
            critGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.NumericUpDown paramUpDown;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown percentUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkRun;
        private ComboBox AlgNameComboBox;
        private Label lblAlgName;
        private GroupBox critGroupBox;
        private RadioButton rBtnPointReduct;
        private RadioButton rBtnParam;
    }
}
