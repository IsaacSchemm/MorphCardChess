namespace Morph.WinForms
{
    partial class ScoreControl
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
            tableLayoutPanel1 = new TableLayoutPanel();
            chkClubs = new CheckBox();
            chkHearts = new CheckBox();
            lblScore = new Label();
            lblDiamonds = new Label();
            lblClubs = new Label();
            lblHearts = new Label();
            chkDiamonds = new CheckBox();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.AutoEllipsis = true;
            lblName.Dock = DockStyle.Fill;
            lblName.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblName.Location = new Point(0, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(116, 48);
            lblName.TabIndex = 0;
            lblName.Text = "Name";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 8;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.Controls.Add(chkClubs, 2, 0);
            tableLayoutPanel1.Controls.Add(chkHearts, 0, 0);
            tableLayoutPanel1.Controls.Add(lblScore, 7, 0);
            tableLayoutPanel1.Controls.Add(lblDiamonds, 5, 0);
            tableLayoutPanel1.Controls.Add(lblClubs, 3, 0);
            tableLayoutPanel1.Controls.Add(lblHearts, 1, 0);
            tableLayoutPanel1.Controls.Add(chkDiamonds, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Right;
            tableLayoutPanel1.Location = new Point(116, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(384, 48);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // chkClubs
            // 
            chkClubs.Appearance = Appearance.Button;
            chkClubs.AutoSize = true;
            chkClubs.Dock = DockStyle.Fill;
            chkClubs.Enabled = false;
            chkClubs.Font = new Font("Segoe UI", 18F);
            chkClubs.Location = new Point(99, 3);
            chkClubs.Name = "chkClubs";
            chkClubs.Size = new Size(42, 42);
            chkClubs.TabIndex = 10;
            chkClubs.Text = "♣";
            chkClubs.TextAlign = ContentAlignment.MiddleCenter;
            chkClubs.UseVisualStyleBackColor = true;
            // 
            // chkHearts
            // 
            chkHearts.Appearance = Appearance.Button;
            chkHearts.AutoSize = true;
            chkHearts.Dock = DockStyle.Fill;
            chkHearts.Enabled = false;
            chkHearts.Font = new Font("Segoe UI", 18F);
            chkHearts.Location = new Point(3, 3);
            chkHearts.Name = "chkHearts";
            chkHearts.Size = new Size(42, 42);
            chkHearts.TabIndex = 9;
            chkHearts.Text = "♥";
            chkHearts.TextAlign = ContentAlignment.MiddleCenter;
            chkHearts.UseVisualStyleBackColor = true;
            // 
            // lblScore
            // 
            lblScore.AutoEllipsis = true;
            lblScore.Dock = DockStyle.Fill;
            lblScore.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblScore.Location = new Point(339, 0);
            lblScore.Name = "lblScore";
            lblScore.Size = new Size(42, 48);
            lblScore.TabIndex = 7;
            lblScore.Text = "0";
            lblScore.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDiamonds
            // 
            lblDiamonds.AutoEllipsis = true;
            lblDiamonds.Dock = DockStyle.Fill;
            lblDiamonds.Font = new Font("Segoe UI", 12F);
            lblDiamonds.Location = new Point(243, 0);
            lblDiamonds.Name = "lblDiamonds";
            lblDiamonds.Size = new Size(42, 48);
            lblDiamonds.TabIndex = 5;
            lblDiamonds.Text = "0";
            lblDiamonds.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblClubs
            // 
            lblClubs.AutoEllipsis = true;
            lblClubs.Dock = DockStyle.Fill;
            lblClubs.Font = new Font("Segoe UI", 12F);
            lblClubs.Location = new Point(147, 0);
            lblClubs.Name = "lblClubs";
            lblClubs.Size = new Size(42, 48);
            lblClubs.TabIndex = 3;
            lblClubs.Text = "0";
            lblClubs.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHearts
            // 
            lblHearts.AutoEllipsis = true;
            lblHearts.Dock = DockStyle.Fill;
            lblHearts.Font = new Font("Segoe UI", 12F);
            lblHearts.Location = new Point(51, 0);
            lblHearts.Name = "lblHearts";
            lblHearts.Size = new Size(42, 48);
            lblHearts.TabIndex = 1;
            lblHearts.Text = "0";
            lblHearts.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chkDiamonds
            // 
            chkDiamonds.Appearance = Appearance.Button;
            chkDiamonds.AutoSize = true;
            chkDiamonds.Dock = DockStyle.Fill;
            chkDiamonds.Enabled = false;
            chkDiamonds.Font = new Font("Segoe UI", 18F);
            chkDiamonds.Location = new Point(195, 3);
            chkDiamonds.Name = "chkDiamonds";
            chkDiamonds.Size = new Size(42, 42);
            chkDiamonds.TabIndex = 8;
            chkDiamonds.Text = "♦";
            chkDiamonds.TextAlign = ContentAlignment.MiddleCenter;
            chkDiamonds.UseVisualStyleBackColor = true;
            // 
            // ScoreControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblName);
            Controls.Add(tableLayoutPanel1);
            Name = "ScoreControl";
            Size = new Size(500, 48);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label lblName;
        private TableLayoutPanel tableLayoutPanel1;
        private Label lblHearts;
        private Label lblScore;
        private Label lblDiamonds;
        private Label lblClubs;
        private CheckBox chkClubs;
        private CheckBox chkHearts;
        private CheckBox chkDiamonds;
    }
}
