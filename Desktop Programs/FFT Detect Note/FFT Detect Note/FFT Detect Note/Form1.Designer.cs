﻿namespace FFT_Detect_Note {
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
            this.btnStart = new System.Windows.Forms.Button();
            this.lblFreq = new System.Windows.Forms.Label();
            this.lblNote = new System.Windows.Forms.Label();
            this.picGraph = new System.Windows.Forms.PictureBox();
            this.lblFFT = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblFreq
            // 
            this.lblFreq.AutoSize = true;
            this.lblFreq.Location = new System.Drawing.Point(12, 53);
            this.lblFreq.Name = "lblFreq";
            this.lblFreq.Size = new System.Drawing.Size(63, 13);
            this.lblFreq.TabIndex = 1;
            this.lblFreq.Text = "Frequency: ";
            // 
            // lblNote
            // 
            this.lblNote.AutoSize = true;
            this.lblNote.Location = new System.Drawing.Point(12, 86);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(73, 13);
            this.lblNote.TabIndex = 2;
            this.lblNote.Text = "Current Note: ";
            // 
            // picGraph
            // 
            this.picGraph.Location = new System.Drawing.Point(15, 102);
            this.picGraph.Name = "picGraph";
            this.picGraph.Size = new System.Drawing.Size(1024, 585);
            this.picGraph.TabIndex = 3;
            this.picGraph.TabStop = false;
            // 
            // lblFFT
            // 
            this.lblFFT.AutoSize = true;
            this.lblFFT.Location = new System.Drawing.Point(142, 53);
            this.lblFFT.Name = "lblFFT";
            this.lblFFT.Size = new System.Drawing.Size(36, 13);
            this.lblFFT.TabIndex = 4;
            this.lblFFT.Text = "lblFFT";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 858);
            this.Controls.Add(this.lblFFT);
            this.Controls.Add(this.picGraph);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.lblFreq);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblFreq;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.PictureBox picGraph;
        private System.Windows.Forms.Label lblFFT;
    }
}

