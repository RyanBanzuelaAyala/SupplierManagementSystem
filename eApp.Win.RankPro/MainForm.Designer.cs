namespace eApp.Win.RankPro
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
            if(disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            this.tme = new System.Windows.Forms.Timer(this.components);
            this.tmx = new System.Windows.Forms.Timer(this.components);
            this.rtBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tme
            // 
            this.tme.Enabled = true;
            this.tme.Interval = 1000;
            this.tme.Tick += new System.EventHandler(this.tme_Tick);
            // 
            // tmx
            // 
            this.tmx.Enabled = true;
            this.tmx.Interval = 1000;
            this.tmx.Tick += new System.EventHandler(this.tmx_Tick);
            // 
            // rtBox
            // 
            this.rtBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtBox.Font = new System.Drawing.Font("Segoe UI Light", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtBox.Location = new System.Drawing.Point(0, 23);
            this.rtBox.Name = "rtBox";
            this.rtBox.ReadOnly = true;
            this.rtBox.Size = new System.Drawing.Size(357, 136);
            this.rtBox.TabIndex = 749;
            this.rtBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Location = new System.Drawing.Point(6, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 856;
            this.label1.Text = "DANUBE [ RANK PROCESS ]";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(357, 162);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tme;
        private System.Windows.Forms.Timer tmx;
        public System.Windows.Forms.RichTextBox rtBox;
        private System.Windows.Forms.Label label1;
    }
}

