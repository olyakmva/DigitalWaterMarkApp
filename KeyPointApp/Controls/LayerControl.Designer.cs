namespace MainForm.Controls
{
    partial class LayerControl
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
            layerCheckBox = new CheckBox();
            lblPoints = new Label();
            btnSave = new Button();
            SuspendLayout();
            // 
            // layerCheckBox
            // 
            layerCheckBox.AutoSize = true;
            layerCheckBox.Location = new Point(3, 9);
            layerCheckBox.Margin = new Padding(3, 4, 3, 4);
            layerCheckBox.Name = "layerCheckBox";
            layerCheckBox.Size = new Size(63, 24);
            layerCheckBox.TabIndex = 0;
            layerCheckBox.Text = "layer";
            layerCheckBox.UseVisualStyleBackColor = true;
            layerCheckBox.CheckedChanged += LayerCheckBoxCheckedChanged;
            // 
            // lblPoints
            // 
            lblPoints.AutoSize = true;
            lblPoints.Location = new Point(179, 9);
            lblPoints.Name = "lblPoints";
            lblPoints.Size = new Size(27, 20);
            lblPoints.TabIndex = 1;
            lblPoints.Text = "n=";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(261, 4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(61, 29);
            btnSave.TabIndex = 2;
            btnSave.Text = "save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += BtnSave_Click;
            // 
            // LayerControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(btnSave);
            Controls.Add(lblPoints);
            Controls.Add(layerCheckBox);
            Margin = new Padding(3, 4, 3, 4);
            Name = "LayerControl";
            Size = new Size(325, 40);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckBox layerCheckBox;
        private Label lblPoints;
        private Button btnSave;
    }
}
