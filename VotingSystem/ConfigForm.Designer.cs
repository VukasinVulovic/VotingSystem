namespace App
{
    partial class ConfigForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.lbl_dbName = new System.Windows.Forms.Label();
            this.pnl_password = new System.Windows.Forms.Panel();
            this.btn_decrypt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pb_eye = new System.Windows.Forms.PictureBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.lbl_voters = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_candidates = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gb_candidates = new System.Windows.Forms.GroupBox();
            this.pnl_candidates = new System.Windows.Forms.Panel();
            this.ofd_database = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_accept = new System.Windows.Forms.Button();
            this.pb_progress = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.pnl_password.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_eye)).BeginInit();
            this.gb_candidates.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.lbl_dbName);
            this.groupBox1.Controls.Add(this.pnl_password);
            this.groupBox1.Location = new System.Drawing.Point(15, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(609, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Baza Podataka";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(9, 23);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 35);
            this.button3.TabIndex = 9;
            this.button3.Text = "Učitaj";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.LoadDB);
            // 
            // lbl_dbName
            // 
            this.lbl_dbName.AutoSize = true;
            this.lbl_dbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dbName.Location = new System.Drawing.Point(101, 32);
            this.lbl_dbName.Name = "lbl_dbName";
            this.lbl_dbName.Size = new System.Drawing.Size(87, 18);
            this.lbl_dbName.TabIndex = 8;
            this.lbl_dbName.Text = "Nije Učitana";
            // 
            // pnl_password
            // 
            this.pnl_password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_password.Controls.Add(this.btn_decrypt);
            this.pnl_password.Controls.Add(this.label1);
            this.pnl_password.Controls.Add(this.pb_eye);
            this.pnl_password.Controls.Add(this.tb_password);
            this.pnl_password.Enabled = false;
            this.pnl_password.Location = new System.Drawing.Point(210, 18);
            this.pnl_password.Name = "pnl_password";
            this.pnl_password.Size = new System.Drawing.Size(390, 45);
            this.pnl_password.TabIndex = 7;
            // 
            // btn_decrypt
            // 
            this.btn_decrypt.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn_decrypt.Enabled = false;
            this.btn_decrypt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_decrypt.Location = new System.Drawing.Point(282, -1);
            this.btn_decrypt.Name = "btn_decrypt";
            this.btn_decrypt.Size = new System.Drawing.Size(108, 45);
            this.btn_decrypt.TabIndex = 16;
            this.btn_decrypt.Text = "Dekriptuj";
            this.btn_decrypt.UseVisualStyleBackColor = false;
            this.btn_decrypt.Click += new System.EventHandler(this.Decrypt);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 24);
            this.label1.TabIndex = 15;
            this.label1.Text = "Šifra:";
            // 
            // pb_eye
            // 
            this.pb_eye.Image = global::App.Properties.Resources.eye;
            this.pb_eye.Location = new System.Drawing.Point(243, 7);
            this.pb_eye.Name = "pb_eye";
            this.pb_eye.Size = new System.Drawing.Size(32, 29);
            this.pb_eye.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pb_eye.TabIndex = 14;
            this.pb_eye.TabStop = false;
            this.pb_eye.Click += new System.EventHandler(this.showHidePassword);
            // 
            // tb_password
            // 
            this.tb_password.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tb_password.Location = new System.Drawing.Point(55, 7);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(180, 29);
            this.tb_password.TabIndex = 13;
            this.tb_password.KeyUp += new System.Windows.Forms.KeyEventHandler(this.checkPasswordLength);
            // 
            // lbl_voters
            // 
            this.lbl_voters.AutoSize = true;
            this.lbl_voters.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_voters.Location = new System.Drawing.Point(107, 447);
            this.lbl_voters.Name = "lbl_voters";
            this.lbl_voters.Size = new System.Drawing.Size(17, 18);
            this.lbl_voters.TabIndex = 5;
            this.lbl_voters.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 447);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Broj Glasača:";
            // 
            // lbl_candidates
            // 
            this.lbl_candidates.AutoSize = true;
            this.lbl_candidates.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_candidates.Location = new System.Drawing.Point(248, 447);
            this.lbl_candidates.Name = "lbl_candidates";
            this.lbl_candidates.Size = new System.Drawing.Size(17, 18);
            this.lbl_candidates.TabIndex = 7;
            this.lbl_candidates.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(143, 447);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Broj Kandidata:";
            // 
            // gb_candidates
            // 
            this.gb_candidates.Controls.Add(this.pnl_candidates);
            this.gb_candidates.Location = new System.Drawing.Point(15, 81);
            this.gb_candidates.Margin = new System.Windows.Forms.Padding(6);
            this.gb_candidates.Name = "gb_candidates";
            this.gb_candidates.Padding = new System.Windows.Forms.Padding(6);
            this.gb_candidates.Size = new System.Drawing.Size(703, 360);
            this.gb_candidates.TabIndex = 2;
            this.gb_candidates.TabStop = false;
            this.gb_candidates.Text = "Kandidati";
            // 
            // pnl_candidates
            // 
            this.pnl_candidates.AutoScroll = true;
            this.pnl_candidates.Location = new System.Drawing.Point(6, 22);
            this.pnl_candidates.Name = "pnl_candidates";
            this.pnl_candidates.Size = new System.Drawing.Size(691, 339);
            this.pnl_candidates.TabIndex = 0;
            // 
            // ofd_database
            // 
            this.ofd_database.Filter = "Database file|*.db";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkRed;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(637, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 27);
            this.button1.TabIndex = 8;
            this.button1.Text = "Izađi";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btn_accept
            // 
            this.btn_accept.BackColor = System.Drawing.Color.Green;
            this.btn_accept.Enabled = false;
            this.btn_accept.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_accept.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_accept.Location = new System.Drawing.Point(638, 48);
            this.btn_accept.Name = "btn_accept";
            this.btn_accept.Size = new System.Drawing.Size(83, 27);
            this.btn_accept.TabIndex = 9;
            this.btn_accept.Text = "Potvrdi";
            this.btn_accept.UseVisualStyleBackColor = false;
            this.btn_accept.Click += new System.EventHandler(this.Save);
            // 
            // pb_progress
            // 
            this.pb_progress.Location = new System.Drawing.Point(15, 442);
            this.pb_progress.Name = "pb_progress";
            this.pb_progress.Size = new System.Drawing.Size(705, 23);
            this.pb_progress.TabIndex = 11;
            this.pb_progress.Visible = false;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 478);
            this.Controls.Add(this.pb_progress);
            this.Controls.Add(this.btn_accept);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gb_candidates);
            this.Controls.Add(this.lbl_candidates);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_voters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(749, 517);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(749, 517);
            this.Name = "ConfigForm";
            this.Text = "Konfiguracija";
            this.Load += new System.EventHandler(this.Init);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnl_password.ResumeLayout(false);
            this.pnl_password.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_eye)).EndInit();
            this.gb_candidates.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnl_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pb_eye;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lbl_dbName;
        private System.Windows.Forms.Label lbl_voters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_decrypt;
        private System.Windows.Forms.Label lbl_candidates;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gb_candidates;
        private System.Windows.Forms.OpenFileDialog ofd_database;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_accept;
        private System.Windows.Forms.Panel pnl_candidates;
        private System.Windows.Forms.ProgressBar pb_progress;
    }
}