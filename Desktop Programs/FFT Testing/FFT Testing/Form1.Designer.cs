namespace FFT_Testing {
    partial class Form1 {
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
            this.components = new System.ComponentModel.Container();
            this.btnAudioInputStart = new System.Windows.Forms.Button();
            this.picGraph = new System.Windows.Forms.PictureBox();
            this.tmr1 = new System.Windows.Forms.Timer(this.components);
            this.btnFFT = new System.Windows.Forms.Button();
            this.btnTestFFT = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblBiggestPitch = new System.Windows.Forms.Label();
            this.btnFindPitch = new System.Windows.Forms.Button();
            this.btnSimFreq = new System.Windows.Forms.Button();
            this.txtFreqToSim = new System.Windows.Forms.TextBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.txtFreqList = new System.Windows.Forms.TextBox();
            this.btnSimulateList = new System.Windows.Forms.Button();
            this.txtSimOutput = new System.Windows.Forms.TextBox();
            this.btnFindNote = new System.Windows.Forms.Button();
            this.btnLoadDict = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAudioInputStart
            // 
            this.btnAudioInputStart.Location = new System.Drawing.Point(12, 12);
            this.btnAudioInputStart.Name = "btnAudioInputStart";
            this.btnAudioInputStart.Size = new System.Drawing.Size(104, 52);
            this.btnAudioInputStart.TabIndex = 0;
            this.btnAudioInputStart.Text = "Start Inputting audio";
            this.btnAudioInputStart.UseVisualStyleBackColor = true;
            this.btnAudioInputStart.Click += new System.EventHandler(this.btnAudioInputStart_Click);
            // 
            // picGraph
            // 
            this.picGraph.Location = new System.Drawing.Point(13, 71);
            this.picGraph.Name = "picGraph";
            this.picGraph.Size = new System.Drawing.Size(1024, 512);
            this.picGraph.TabIndex = 1;
            this.picGraph.TabStop = false;
            // 
            // tmr1
            // 
            this.tmr1.Tick += new System.EventHandler(this.tmr1_Tick);
            // 
            // btnFFT
            // 
            this.btnFFT.Location = new System.Drawing.Point(145, 13);
            this.btnFFT.Name = "btnFFT";
            this.btnFFT.Size = new System.Drawing.Size(75, 23);
            this.btnFFT.TabIndex = 2;
            this.btnFFT.Text = "FFT";
            this.btnFFT.UseVisualStyleBackColor = true;
            this.btnFFT.Click += new System.EventHandler(this.btnFFT_Click);
            // 
            // btnTestFFT
            // 
            this.btnTestFFT.Location = new System.Drawing.Point(278, 12);
            this.btnTestFFT.Name = "btnTestFFT";
            this.btnTestFFT.Size = new System.Drawing.Size(75, 23);
            this.btnTestFFT.TabIndex = 3;
            this.btnTestFFT.Text = "Ideal FFT";
            this.btnTestFFT.UseVisualStyleBackColor = true;
            this.btnTestFFT.Click += new System.EventHandler(this.btnTestFFT_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(397, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear Display";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblBiggestPitch
            // 
            this.lblBiggestPitch.AutoSize = true;
            this.lblBiggestPitch.Location = new System.Drawing.Point(512, 51);
            this.lblBiggestPitch.Name = "lblBiggestPitch";
            this.lblBiggestPitch.Size = new System.Drawing.Size(76, 13);
            this.lblBiggestPitch.TabIndex = 5;
            this.lblBiggestPitch.Text = "lblBiggestPitch";
            // 
            // btnFindPitch
            // 
            this.btnFindPitch.Location = new System.Drawing.Point(515, 13);
            this.btnFindPitch.Name = "btnFindPitch";
            this.btnFindPitch.Size = new System.Drawing.Size(121, 23);
            this.btnFindPitch.TabIndex = 6;
            this.btnFindPitch.Text = "Find Biggest Pitch";
            this.btnFindPitch.UseVisualStyleBackColor = true;
            this.btnFindPitch.Click += new System.EventHandler(this.btnFindPitch_Click);
            // 
            // btnSimFreq
            // 
            this.btnSimFreq.Location = new System.Drawing.Point(789, 8);
            this.btnSimFreq.Name = "btnSimFreq";
            this.btnSimFreq.Size = new System.Drawing.Size(155, 23);
            this.btnSimFreq.TabIndex = 7;
            this.btnSimFreq.Text = "Simulate Freq";
            this.btnSimFreq.UseVisualStyleBackColor = true;
            this.btnSimFreq.Click += new System.EventHandler(this.btnSimFreq_Click);
            // 
            // txtFreqToSim
            // 
            this.txtFreqToSim.Location = new System.Drawing.Point(673, 11);
            this.txtFreqToSim.Name = "txtFreqToSim";
            this.txtFreqToSim.Size = new System.Drawing.Size(100, 20);
            this.txtFreqToSim.TabIndex = 8;
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(612, 53);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(35, 13);
            this.lblNote.TabIndex = 9;
            this.lblNote.Text = "label1";
            // 
            // txtFreqList
            // 
            this.txtFreqList.Location = new System.Drawing.Point(1058, 71);
            this.txtFreqList.Multiline = true;
            this.txtFreqList.Name = "txtFreqList";
            this.txtFreqList.Size = new System.Drawing.Size(341, 154);
            this.txtFreqList.TabIndex = 10;
            // 
            // btnSimulateList
            // 
            this.btnSimulateList.Location = new System.Drawing.Point(1058, 504);
            this.btnSimulateList.Name = "btnSimulateList";
            this.btnSimulateList.Size = new System.Drawing.Size(75, 23);
            this.btnSimulateList.TabIndex = 11;
            this.btnSimulateList.Text = "Simulate List";
            this.btnSimulateList.UseVisualStyleBackColor = true;
            this.btnSimulateList.Click += new System.EventHandler(this.btnSimulateList_Click);
            // 
            // txtSimOutput
            // 
            this.txtSimOutput.Location = new System.Drawing.Point(1058, 232);
            this.txtSimOutput.Multiline = true;
            this.txtSimOutput.Name = "txtSimOutput";
            this.txtSimOutput.Size = new System.Drawing.Size(341, 241);
            this.txtSimOutput.TabIndex = 12;
            // 
            // btnFindNote
            // 
            this.btnFindNote.Location = new System.Drawing.Point(736, 40);
            this.btnFindNote.Name = "btnFindNote";
            this.btnFindNote.Size = new System.Drawing.Size(75, 23);
            this.btnFindNote.TabIndex = 13;
            this.btnFindNote.Text = "Find Note";
            this.btnFindNote.UseVisualStyleBackColor = true;
            this.btnFindNote.Click += new System.EventHandler(this.btnFindNote_Click);
            // 
            // btnLoadDict
            // 
            this.btnLoadDict.Location = new System.Drawing.Point(880, 38);
            this.btnLoadDict.Name = "btnLoadDict";
            this.btnLoadDict.Size = new System.Drawing.Size(75, 23);
            this.btnLoadDict.TabIndex = 14;
            this.btnLoadDict.Text = "LoadDictionary";
            this.btnLoadDict.UseVisualStyleBackColor = true;
            this.btnLoadDict.Click += new System.EventHandler(this.btnLoadDict_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 597);
            this.Controls.Add(this.btnLoadDict);
            this.Controls.Add(this.btnFindNote);
            this.Controls.Add(this.txtSimOutput);
            this.Controls.Add(this.btnSimulateList);
            this.Controls.Add(this.txtFreqList);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.txtFreqToSim);
            this.Controls.Add(this.btnSimFreq);
            this.Controls.Add(this.btnFindPitch);
            this.Controls.Add(this.lblBiggestPitch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnTestFFT);
            this.Controls.Add(this.btnFFT);
            this.Controls.Add(this.picGraph);
            this.Controls.Add(this.btnAudioInputStart);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAudioInputStart;
        private System.Windows.Forms.PictureBox picGraph;
        private System.Windows.Forms.Timer tmr1;
        private System.Windows.Forms.Button btnFFT;
        private System.Windows.Forms.Button btnTestFFT;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblBiggestPitch;
        private System.Windows.Forms.Button btnFindPitch;
        private System.Windows.Forms.Button btnSimFreq;
        private System.Windows.Forms.TextBox txtFreqToSim;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.TextBox txtFreqList;
        private System.Windows.Forms.Button btnSimulateList;
        private System.Windows.Forms.TextBox txtSimOutput;
        private System.Windows.Forms.Button btnFindNote;
        private System.Windows.Forms.Button btnLoadDict;
    }
}

