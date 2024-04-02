namespace Morph
{
    partial class Form1
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
            panel1 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new Panel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            label3 = new Label();
            txtDarkScore = new Label();
            panel2 = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            txtLightScore = new Label();
            btnUndo = new Button();
            btnRedo = new Button();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            panel2.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(tableLayoutPanel3);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(760, 808);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 8;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 92);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 8;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.Size = new Size(760, 624);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 32);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(760, 60);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 716);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(760, 60);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Controls.Add(flowLayoutPanel2);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 776);
            panel3.Name = "panel3";
            panel3.Size = new Size(760, 32);
            panel3.TabIndex = 4;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.Controls.Add(label3);
            flowLayoutPanel2.Controls.Add(txtDarkScore);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(0, 0);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(760, 32);
            flowLayoutPanel2.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 18F);
            label3.Location = new Point(3, 0);
            label3.Name = "label3";
            label3.Size = new Size(98, 32);
            label3.TabIndex = 2;
            label3.Text = "Player 1";
            // 
            // txtDarkScore
            // 
            txtDarkScore.AutoSize = true;
            txtDarkScore.Font = new Font("Segoe UI", 18F);
            txtDarkScore.Location = new Point(107, 0);
            txtDarkScore.Name = "txtDarkScore";
            txtDarkScore.Size = new Size(76, 32);
            txtDarkScore.TabIndex = 3;
            txtDarkScore.Text = "♠ ♠ ♠";
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.Controls.Add(flowLayoutPanel1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(760, 32);
            panel2.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(txtLightScore);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(760, 32);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(98, 32);
            label1.TabIndex = 0;
            label1.Text = "Player 2";
            // 
            // txtLightScore
            // 
            txtLightScore.AutoSize = true;
            txtLightScore.Font = new Font("Segoe UI", 18F);
            txtLightScore.Location = new Point(107, 0);
            txtLightScore.Name = "txtLightScore";
            txtLightScore.Size = new Size(76, 32);
            txtLightScore.TabIndex = 1;
            txtLightScore.Text = "♠ ♠ ♠";
            // 
            // btnUndo
            // 
            btnUndo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnUndo.Location = new Point(12, 826);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(75, 23);
            btnUndo.TabIndex = 1;
            btnUndo.Text = "Undo";
            btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnRedo
            // 
            btnRedo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRedo.Location = new Point(697, 826);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(75, 23);
            btnRedo.TabIndex = 2;
            btnRedo.Text = "Redo";
            btnRedo.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 861);
            Controls.Add(btnRedo);
            Controls.Add(btnUndo);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnUndo;
        private Button btnRedo;
        private Panel panel3;
        private Label txtDarkScore;
        private Label label3;
        private Panel panel2;
        private Label txtLightScore;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
