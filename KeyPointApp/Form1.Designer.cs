namespace KeyPointApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            mainContainer = new SplitContainer();
            btnProcess = new Button();
            mapPictureBox = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainContainer).BeginInit();
            mainContainer.Panel1.SuspendLayout();
            mainContainer.Panel2.SuspendLayout();
            mainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mapPictureBox).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, clearToolStripMenuItem, exitToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 2, 0, 2);
            menuStrip1.Size = new Size(1622, 33);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(158, 34);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(158, 34);
            saveToolStripMenuItem.Text = "Save";
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(67, 29);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += ClearToolStripMenuItemClick;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(55, 29);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // mainContainer
            // 
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.Location = new Point(0, 33);
            mainContainer.Margin = new Padding(4, 4, 4, 4);
            mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            mainContainer.Panel1.AutoScroll = true;
            mainContainer.Panel1.BackColor = SystemColors.GradientInactiveCaption;
            mainContainer.Panel1.Controls.Add(btnProcess);
            mainContainer.Panel1.Resize += MapSplitContainerPanel1Resize;
            // 
            // mainContainer.Panel2
            // 
            mainContainer.Panel2.Controls.Add(mapPictureBox);
            mainContainer.Size = new Size(1622, 636);
            mainContainer.SplitterDistance = 248;
            mainContainer.SplitterWidth = 5;
            mainContainer.TabIndex = 1;
            // 
            // btnProcess
            // 
            btnProcess.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnProcess.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            btnProcess.Location = new Point(0, 500);
            btnProcess.Margin = new Padding(4, 4, 4, 4);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(244, 56);
            btnProcess.TabIndex = 0;
            btnProcess.Text = "Process";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += BtnProcessClick;
            // 
            // mapPictureBox
            // 
            mapPictureBox.Dock = DockStyle.Fill;
            mapPictureBox.Location = new Point(0, 0);
            mapPictureBox.Margin = new Padding(4, 4, 4, 4);
            mapPictureBox.Name = "mapPictureBox";
            mapPictureBox.Size = new Size(1369, 636);
            mapPictureBox.TabIndex = 0;
            mapPictureBox.TabStop = false;
            mapPictureBox.Paint += MapPictureBoxPaint;
            mapPictureBox.MouseDoubleClick += MapPictureBoxMouseDoubleClick;
            mapPictureBox.MouseDown += MapPictureBoxMouseDown;
            mapPictureBox.MouseEnter += MapPictureBoxMouseEnter;
            mapPictureBox.MouseLeave += MapPictureBoxMouseLeave;
            mapPictureBox.MouseMove += MapPictureBoxMouseMove;
            mapPictureBox.MouseUp += MapPictureBoxMouseUp;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1622, 669);
            Controls.Add(mainContainer);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 4, 4, 4);
            Name = "MainForm";
            Text = "Conflation";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            mainContainer.Panel1.ResumeLayout(false);
            mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainContainer).EndInit();
            mainContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mapPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private SplitContainer mainContainer;
        private PictureBox mapPictureBox;
        private Button btnProcess;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}