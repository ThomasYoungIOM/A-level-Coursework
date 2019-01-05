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
            this.lblInstrument = new System.Windows.Forms.Label();
            this.lblDifficulty = new System.Windows.Forms.Label();
            this.lblSongName = new System.Windows.Forms.Label();
            this.ckbMetronome = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbCursor = new System.Windows.Forms.CheckBox();
            this.ckbFingering = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnMark = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // picDisplay
            // 
            this.picDisplay.Location = new System.Drawing.Point(16, 107);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(760, 700);
            this.picDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDisplay.TabIndex = 13;
            this.picDisplay.TabStop = false;
            // 
            // lblInstrument
            // 
            this.lblInstrument.AutoSize = true;
            this.lblInstrument.Location = new System.Drawing.Point(342, 15);
            this.lblInstrument.Name = "lblInstrument";
            this.lblInstrument.Size = new System.Drawing.Size(62, 13);
            this.lblInstrument.TabIndex = 9;
            this.lblInstrument.Text = "Instrument: ";
            // 
            // lblDifficulty
            // 
            this.lblDifficulty.AutoSize = true;
            this.lblDifficulty.Location = new System.Drawing.Point(180, 15);
            this.lblDifficulty.Name = "lblDifficulty";
            this.lblDifficulty.Size = new System.Drawing.Size(53, 13);
            this.lblDifficulty.TabIndex = 8;
            this.lblDifficulty.Text = "Difficulty: ";
            // 
            // lblSongName
            // 
            this.lblSongName.AutoSize = true;
            this.lblSongName.Location = new System.Drawing.Point(13, 15);
            this.lblSongName.Name = "lblSongName";
            this.lblSongName.Size = new System.Drawing.Size(69, 13);
            this.lblSongName.TabIndex = 7;
            this.lblSongName.Text = "Song Name: ";
            // 
            // ckbMetronome
            // 
            this.ckbMetronome.AutoSize = true;
            this.ckbMetronome.Location = new System.Drawing.Point(87, 55);
            this.ckbMetronome.Name = "ckbMetronome";
            this.ckbMetronome.Size = new System.Drawing.Size(79, 17);
            this.ckbMetronome.TabIndex = 14;
            this.ckbMetronome.Text = "Metronome";
            this.ckbMetronome.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Helping Aids:";
            // 
            // ckbCursor
            // 
            this.ckbCursor.AutoSize = true;
            this.ckbCursor.Location = new System.Drawing.Point(183, 56);
            this.ckbCursor.Name = "ckbCursor";
            this.ckbCursor.Size = new System.Drawing.Size(56, 17);
            this.ckbCursor.TabIndex = 16;
            this.ckbCursor.Text = "Cursor";
            this.ckbCursor.UseVisualStyleBackColor = true;
            // 
            // ckbFingering
            // 
            this.ckbFingering.AutoSize = true;
            this.ckbFingering.Location = new System.Drawing.Point(261, 56);
            this.ckbFingering.Name = "ckbFingering";
            this.ckbFingering.Size = new System.Drawing.Size(97, 17);
            this.ckbFingering.TabIndex = 17;
            this.ckbFingering.Text = "Fingering Chart";
            this.ckbFingering.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(420, 56);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 18;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(508, 56);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 19;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnMark
            // 
            this.btnMark.Location = new System.Drawing.Point(596, 56);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(75, 23);
            this.btnMark.TabIndex = 20;
            this.btnMark.Text = "Mark";
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
            this.Controls.Add(this.ckbCursor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckbMetronome);
            this.Controls.Add(this.picDisplay);
            this.Controls.Add(this.lblInstrument);
            this.Controls.Add(this.lblDifficulty);
            this.Controls.Add(this.lblSongName);
            this.Name = "frmTest";
            this.Text = "frmTest";
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.Label lblInstrument;
        private System.Windows.Forms.Label lblDifficulty;
        private System.Windows.Forms.Label lblSongName;
        private System.Windows.Forms.CheckBox ckbMetronome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbCursor;
        private System.Windows.Forms.CheckBox ckbFingering;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnMark;
    }
}