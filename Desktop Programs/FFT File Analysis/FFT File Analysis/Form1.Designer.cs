namespace FFT_File_Analysis {
    partial class frmMain {
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
            this.btnRec = new System.Windows.Forms.Button();
            this.btnOpenWav = new System.Windows.Forms.Button();
            this.ofdWavFile = new System.Windows.Forms.OpenFileDialog();
            this.picOutput = new System.Windows.Forms.PictureBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnDrawValues = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRec
            // 
            this.btnRec.Location = new System.Drawing.Point(13, 13);
            this.btnRec.Name = "btnRec";
            this.btnRec.Size = new System.Drawing.Size(96, 49);
            this.btnRec.TabIndex = 0;
            this.btnRec.Text = "Start/Stop Recording";
            this.btnRec.UseVisualStyleBackColor = true;
            this.btnRec.Click += new System.EventHandler(this.btnRec_Click);
            // 
            // btnOpenWav
            // 
            this.btnOpenWav.Location = new System.Drawing.Point(130, 13);
            this.btnOpenWav.Name = "btnOpenWav";
            this.btnOpenWav.Size = new System.Drawing.Size(75, 23);
            this.btnOpenWav.TabIndex = 1;
            this.btnOpenWav.Text = "Open File";
            this.btnOpenWav.UseVisualStyleBackColor = true;
            this.btnOpenWav.Click += new System.EventHandler(this.btnOpenWav_Click);
            // 
            // ofdWavFile
            // 
            this.ofdWavFile.Filter = "All files (*.*) |*.*";
            this.ofdWavFile.InitialDirectory = "C:\\Users\\Thomas\\source\\repos\\A-level Coursework\\Desktop Programs\\FFT File Analysi" +
    "s\\FFT File Analysis\\bin\\Debug\\Wav Files";
            // 
            // picOutput
            // 
            this.picOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picOutput.Location = new System.Drawing.Point(13, 69);
            this.picOutput.Name = "picOutput";
            this.picOutput.Size = new System.Drawing.Size(1024, 512);
            this.picOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picOutput.TabIndex = 2;
            this.picOutput.TabStop = false;
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(225, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(808, 50);
            this.txtOutput.TabIndex = 3;
            // 
            // btnDrawValues
            // 
            this.btnDrawValues.Location = new System.Drawing.Point(130, 40);
            this.btnDrawValues.Name = "btnDrawValues";
            this.btnDrawValues.Size = new System.Drawing.Size(75, 23);
            this.btnDrawValues.TabIndex = 4;
            this.btnDrawValues.Text = "Draw Values";
            this.btnDrawValues.UseVisualStyleBackColor = true;
            this.btnDrawValues.Click += new System.EventHandler(this.btnDrawValues_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 586);
            this.Controls.Add(this.btnDrawValues);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.picOutput);
            this.Controls.Add(this.btnOpenWav);
            this.Controls.Add(this.btnRec);
            this.Name = "frmMain";
            this.Text = "Main Form";
            ((System.ComponentModel.ISupportInitialize)(this.picOutput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRec;
        private System.Windows.Forms.Button btnOpenWav;
        private System.Windows.Forms.OpenFileDialog ofdWavFile;
        private System.Windows.Forms.PictureBox picOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnDrawValues;
    }
}

