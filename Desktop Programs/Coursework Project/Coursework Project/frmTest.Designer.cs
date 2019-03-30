namespace Coursework_Project {
    partial class frmTest {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.ckbFingering = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMark = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // picDisplay
            // 
            this.picDisplay.Location = new System.Drawing.Point(16, 65);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(760, 700);
            this.picDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDisplay.TabIndex = 13;
            this.picDisplay.TabStop = false;
            // 
            // ckbFingering
            // 
            this.ckbFingering.AutoSize = true;
            this.ckbFingering.Location = new System.Drawing.Point(39, 24);
            this.ckbFingering.Name = "ckbFingering";
            this.ckbFingering.Size = new System.Drawing.Size(97, 17);
            this.ckbFingering.TabIndex = 17;
            this.ckbFingering.Text = "Fingering Chart";
            this.ckbFingering.UseVisualStyleBackColor = true;
            this.ckbFingering.CheckedChanged += new System.EventHandler(this.ckbFingering_CheckedChanged);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(301, 18);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(153, 23);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "Start Recording Audio";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(460, 18);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(123, 23);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "Stop Recording";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(596, 18);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(151, 23);
            this.btnMark.TabIndex = 20;
            this.btnMark.Text = "Mark Played Audio";
            this.btnMark.UseVisualStyleBackColor = true;
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 823);
            this.Controls.Add(this.btnMark);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.ckbFingering);
            this.Controls.Add(this.picDisplay);
            this.Name = "frmTest";
            this.Text = "/";
            this.Load += new System.EventHandler(this.frmTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.CheckBox ckbFingering;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnMark;
    }
}