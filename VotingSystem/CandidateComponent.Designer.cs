namespace App
{
    partial class CandidateComponent
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Tb_title = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Tb_description = new System.Windows.Forms.TextBox();
            this.Pb_remove = new System.Windows.Forms.PictureBox();
            this.Pb_add = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_remove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_add)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Tb_title);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Naslov";
            // 
            // Tb_title
            // 
            this.Tb_title.Location = new System.Drawing.Point(9, 25);
            this.Tb_title.Margin = new System.Windows.Forms.Padding(6);
            this.Tb_title.Multiline = true;
            this.Tb_title.Name = "Tb_title";
            this.Tb_title.Size = new System.Drawing.Size(132, 47);
            this.Tb_title.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Tb_description);
            this.groupBox2.Location = new System.Drawing.Point(153, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 81);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Deskripcija";
            // 
            // Tb_description
            // 
            this.Tb_description.Location = new System.Drawing.Point(9, 25);
            this.Tb_description.Margin = new System.Windows.Forms.Padding(6);
            this.Tb_description.Multiline = true;
            this.Tb_description.Name = "Tb_description";
            this.Tb_description.Size = new System.Drawing.Size(422, 47);
            this.Tb_description.TabIndex = 2;
            // 
            // Pb_remove
            // 
            this.Pb_remove.Image = global::App.Properties.Resources.minus_PNG9;
            this.Pb_remove.Location = new System.Drawing.Point(645, 15);
            this.Pb_remove.Name = "Pb_remove";
            this.Pb_remove.Size = new System.Drawing.Size(40, 69);
            this.Pb_remove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pb_remove.TabIndex = 5;
            this.Pb_remove.TabStop = false;
            // 
            // Pb_add
            // 
            this.Pb_add.Image = global::App.Properties.Resources.plus_PNG110;
            this.Pb_add.Location = new System.Drawing.Point(599, 15);
            this.Pb_add.Name = "Pb_add";
            this.Pb_add.Size = new System.Drawing.Size(40, 69);
            this.Pb_add.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Pb_add.TabIndex = 4;
            this.Pb_add.TabStop = false;
            // 
            // CandidateComponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.Pb_remove);
            this.Controls.Add(this.Pb_add);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "CandidateComponent";
            this.Size = new System.Drawing.Size(689, 88);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_remove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_add)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.PictureBox Pb_add;
        public System.Windows.Forms.PictureBox Pb_remove;
        public System.Windows.Forms.TextBox Tb_title;
        public System.Windows.Forms.TextBox Tb_description;
    }
}
