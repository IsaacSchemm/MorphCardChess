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
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnUndo = new Button();
            btnRedo = new Button();
            promotionPanel1 = new Panel();
            label1 = new Label();
            btnPromoLight1 = new Button();
            btnPromoLight2 = new Button();
            promotionPanel2 = new Panel();
            btnPromoDark2 = new Button();
            btnPromoDark1 = new Button();
            label2 = new Label();
            panel1.SuspendLayout();
            promotionPanel1.SuspendLayout();
            promotionPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(tableLayoutPanel3);
            panel1.Controls.Add(promotionPanel2);
            panel1.Controls.Add(promotionPanel1);
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(420, 505);
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
            tableLayoutPanel3.Size = new Size(420, 321);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Dock = DockStyle.Bottom;
            tableLayoutPanel2.Location = new Point(0, 445);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(420, 60);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(420, 60);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btnUndo
            // 
            btnUndo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnUndo.Location = new Point(12, 523);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(75, 23);
            btnUndo.TabIndex = 1;
            btnUndo.Text = "Undo";
            btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnRedo
            // 
            btnRedo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRedo.Location = new Point(357, 523);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(75, 23);
            btnRedo.TabIndex = 2;
            btnRedo.Text = "Redo";
            btnRedo.UseVisualStyleBackColor = true;
            // 
            // promotionPanel1
            // 
            promotionPanel1.AutoSize = true;
            promotionPanel1.Controls.Add(btnPromoLight2);
            promotionPanel1.Controls.Add(btnPromoLight1);
            promotionPanel1.Controls.Add(label1);
            promotionPanel1.Dock = DockStyle.Top;
            promotionPanel1.Location = new Point(0, 60);
            promotionPanel1.Name = "promotionPanel1";
            promotionPanel1.Size = new Size(420, 32);
            promotionPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 10);
            label1.Name = "label1";
            label1.Size = new Size(70, 15);
            label1.TabIndex = 0;
            label1.Text = "Promote to:";
            // 
            // btnPromoLight1
            // 
            btnPromoLight1.Location = new Point(79, 6);
            btnPromoLight1.Name = "btnPromoLight1";
            btnPromoLight1.Size = new Size(75, 23);
            btnPromoLight1.TabIndex = 1;
            btnPromoLight1.UseVisualStyleBackColor = true;
            // 
            // btnPromoLight2
            // 
            btnPromoLight2.Location = new Point(160, 6);
            btnPromoLight2.Name = "btnPromoLight2";
            btnPromoLight2.Size = new Size(75, 23);
            btnPromoLight2.TabIndex = 2;
            btnPromoLight2.UseVisualStyleBackColor = true;
            // 
            // promotionPanel2
            // 
            promotionPanel2.AutoSize = true;
            promotionPanel2.Controls.Add(btnPromoDark2);
            promotionPanel2.Controls.Add(btnPromoDark1);
            promotionPanel2.Controls.Add(label2);
            promotionPanel2.Dock = DockStyle.Bottom;
            promotionPanel2.Location = new Point(0, 413);
            promotionPanel2.Name = "promotionPanel2";
            promotionPanel2.Size = new Size(420, 32);
            promotionPanel2.TabIndex = 1;
            // 
            // btnPromoDark2
            // 
            btnPromoDark2.Location = new Point(160, 6);
            btnPromoDark2.Name = "btnPromoDark2";
            btnPromoDark2.Size = new Size(75, 23);
            btnPromoDark2.TabIndex = 2;
            btnPromoDark2.UseVisualStyleBackColor = true;
            // 
            // btnPromoDark1
            // 
            btnPromoDark1.Location = new Point(79, 6);
            btnPromoDark1.Name = "btnPromoDark1";
            btnPromoDark1.Size = new Size(75, 23);
            btnPromoDark1.TabIndex = 1;
            btnPromoDark1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 10);
            label2.Name = "label2";
            label2.Size = new Size(70, 15);
            label2.TabIndex = 0;
            label2.Text = "Promote to:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(444, 558);
            Controls.Add(btnRedo);
            Controls.Add(btnUndo);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            promotionPanel1.ResumeLayout(false);
            promotionPanel1.PerformLayout();
            promotionPanel2.ResumeLayout(false);
            promotionPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnUndo;
        private Button btnRedo;
        private Panel promotionPanel2;
        private Button btnPromoDark2;
        private Button btnPromoDark1;
        private Label label2;
        private Panel promotionPanel1;
        private Button btnPromoLight2;
        private Button btnPromoLight1;
        private Label label1;
    }
}
