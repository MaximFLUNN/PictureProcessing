namespace PicturesProcessing
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.тыкToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пикчаВанToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ванToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.туToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пикчаТуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mSEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uIQIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mSEAverageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uIQIAverageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.тыкToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(882, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // тыкToolStripMenuItem
            // 
            this.тыкToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пикчаВанToolStripMenuItem,
            this.пикчаТуToolStripMenuItem});
            this.тыкToolStripMenuItem.Name = "тыкToolStripMenuItem";
            this.тыкToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.тыкToolStripMenuItem.Text = "Тык";
            // 
            // пикчаВанToolStripMenuItem
            // 
            this.пикчаВанToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ванToolStripMenuItem,
            this.туToolStripMenuItem});
            this.пикчаВанToolStripMenuItem.Name = "пикчаВанToolStripMenuItem";
            this.пикчаВанToolStripMenuItem.Size = new System.Drawing.Size(168, 26);
            this.пикчаВанToolStripMenuItem.Text = "Пикчи";
            // 
            // ванToolStripMenuItem
            // 
            this.ванToolStripMenuItem.Name = "ванToolStripMenuItem";
            this.ванToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.ванToolStripMenuItem.Text = "Ван";
            this.ванToolStripMenuItem.Click += new System.EventHandler(this.ванToolStripMenuItem_Click);
            // 
            // туToolStripMenuItem
            // 
            this.туToolStripMenuItem.Name = "туToolStripMenuItem";
            this.туToolStripMenuItem.Size = new System.Drawing.Size(118, 26);
            this.туToolStripMenuItem.Text = "Ту";
            this.туToolStripMenuItem.Click += new System.EventHandler(this.туToolStripMenuItem_Click);
            // 
            // пикчаТуToolStripMenuItem
            // 
            this.пикчаТуToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mSEToolStripMenuItem,
            this.uIQIToolStripMenuItem,
            this.mSEAverageToolStripMenuItem,
            this.uIQIAverageToolStripMenuItem});
            this.пикчаТуToolStripMenuItem.Name = "пикчаТуToolStripMenuItem";
            this.пикчаТуToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.пикчаТуToolStripMenuItem.Text = "Обработка";
            // 
            // mSEToolStripMenuItem
            // 
            this.mSEToolStripMenuItem.Name = "mSEToolStripMenuItem";
            this.mSEToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.mSEToolStripMenuItem.Text = "MSE";
            this.mSEToolStripMenuItem.Click += new System.EventHandler(this.mSEToolStripMenuItem_Click);
            // 
            // uIQIToolStripMenuItem
            // 
            this.uIQIToolStripMenuItem.Name = "uIQIToolStripMenuItem";
            this.uIQIToolStripMenuItem.Size = new System.Drawing.Size(121, 26);
            this.uIQIToolStripMenuItem.Text = "UIQI";
            this.uIQIToolStripMenuItem.Click += new System.EventHandler(this.uIQIToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 76);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(410, 360);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(460, 76);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(410, 360);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "MSE: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(457, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "UIQI: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 452);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "MSE average: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(457, 452);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "UIQI average: ";
            // 
            // mSEAverageToolStripMenuItem
            // 
            this.mSEAverageToolStripMenuItem.Name = "mSEAverageToolStripMenuItem";
            this.mSEAverageToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.mSEAverageToolStripMenuItem.Text = "MSE average";
            this.mSEAverageToolStripMenuItem.Click += new System.EventHandler(this.mSEAverageToolStripMenuItem_Click);
            // 
            // uIQIAverageToolStripMenuItem
            // 
            this.uIQIAverageToolStripMenuItem.Name = "uIQIAverageToolStripMenuItem";
            this.uIQIAverageToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.uIQIAverageToolStripMenuItem.Text = "UIQI average";
            this.uIQIAverageToolStripMenuItem.Click += new System.EventHandler(this.uIQIAverageToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 503);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem тыкToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пикчаВанToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ванToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem туToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пикчаТуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mSEToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem uIQIToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem mSEAverageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uIQIAverageToolStripMenuItem;
    }
}

