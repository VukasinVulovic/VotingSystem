namespace ServerZaGlasanje
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_zapocni = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tb_url = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtb_status = new System.Windows.Forms.RichTextBox();
            this.btn_zaustavi = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_zapocni
            // 
            this.btn_zapocni.BackColor = System.Drawing.Color.Green;
            this.btn_zapocni.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_zapocni.ForeColor = System.Drawing.Color.White;
            this.btn_zapocni.Location = new System.Drawing.Point(12, 352);
            this.btn_zapocni.Margin = new System.Windows.Forms.Padding(6);
            this.btn_zapocni.Name = "btn_zapocni";
            this.btn_zapocni.Size = new System.Drawing.Size(128, 62);
            this.btn_zapocni.TabIndex = 0;
            this.btn_zapocni.Text = "Započni Glasanje";
            this.btn_zapocni.UseVisualStyleBackColor = false;
            this.btn_zapocni.Click += new System.EventHandler(this.zapocniGlasanje);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.tb_url);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(684, 56);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "URL za glasanje";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(603, 22);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 28);
            this.button3.TabIndex = 1;
            this.button3.Text = "kopiraj";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.kopirajURL);
            // 
            // tb_url
            // 
            this.tb_url.BackColor = System.Drawing.SystemColors.MenuBar;
            this.tb_url.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_url.Enabled = false;
            this.tb_url.Location = new System.Drawing.Point(6, 23);
            this.tb_url.Name = "tb_url";
            this.tb_url.Size = new System.Drawing.Size(593, 26);
            this.tb_url.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_status);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(684, 276);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status Glasanja";
            // 
            // rtb_status
            // 
            this.rtb_status.BackColor = System.Drawing.SystemColors.MenuBar;
            this.rtb_status.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_status.Location = new System.Drawing.Point(6, 19);
            this.rtb_status.Name = "rtb_status";
            this.rtb_status.Size = new System.Drawing.Size(672, 251);
            this.rtb_status.TabIndex = 0;
            this.rtb_status.Text = "";
            // 
            // btn_zaustavi
            // 
            this.btn_zaustavi.BackColor = System.Drawing.Color.DarkRed;
            this.btn_zaustavi.Enabled = false;
            this.btn_zaustavi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_zaustavi.ForeColor = System.Drawing.Color.White;
            this.btn_zaustavi.Location = new System.Drawing.Point(568, 352);
            this.btn_zaustavi.Margin = new System.Windows.Forms.Padding(6);
            this.btn_zaustavi.Name = "btn_zaustavi";
            this.btn_zaustavi.Size = new System.Drawing.Size(128, 62);
            this.btn_zaustavi.TabIndex = 3;
            this.btn_zaustavi.Text = "Zaustavi Glasanje";
            this.btn_zaustavi.UseVisualStyleBackColor = false;
            this.btn_zaustavi.Click += new System.EventHandler(this.zaustaviGlasanje);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 425);
            this.Controls.Add(this.btn_zaustavi);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_zapocni);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximumSize = new System.Drawing.Size(724, 464);
            this.MinimumSize = new System.Drawing.Size(724, 464);
            this.Name = "MainForm";
            this.Text = "Započni Glasanje";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Close);
            this.Load += new System.EventHandler(this.Init);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_zapocni;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtb_status;
        private System.Windows.Forms.Button btn_zaustavi;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.TextBox tb_url;
    }
}

