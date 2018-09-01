namespace Midi_Serial_sender
{
    partial class Form1
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
            this.txtData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtMidiFileOutput = new System.Windows.Forms.TextBox();
            this.lsvMidi = new System.Windows.Forms.ListView();
            this.btnLsvTest = new System.Windows.Forms.Button();
            this.btnMidiToLSV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(12, 29);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(257, 144);
            this.txtData.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Codes to send to ardunio";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(194, 6);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "mid";
            this.openFileDialog1.Filter = "Midi Files|*.mid";
            this.openFileDialog1.InitialDirectory = "C:\\Users\\Thomas\\Documents\\Computer Science Coursework\\Midi Files";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(318, 29);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(101, 23);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtMidiFileOutput
            // 
            this.txtMidiFileOutput.Location = new System.Drawing.Point(461, 29);
            this.txtMidiFileOutput.Multiline = true;
            this.txtMidiFileOutput.Name = "txtMidiFileOutput";
            this.txtMidiFileOutput.Size = new System.Drawing.Size(800, 144);
            this.txtMidiFileOutput.TabIndex = 4;
            // 
            // lsvMidi
            // 
            this.lsvMidi.AutoArrange = false;
            this.lsvMidi.Location = new System.Drawing.Point(15, 194);
            this.lsvMidi.Name = "lsvMidi";
            this.lsvMidi.Size = new System.Drawing.Size(1108, 230);
            this.lsvMidi.TabIndex = 5;
            this.lsvMidi.UseCompatibleStateImageBehavior = false;
            // 
            // btnLsvTest
            // 
            this.btnLsvTest.Location = new System.Drawing.Point(275, 150);
            this.btnLsvTest.Name = "btnLsvTest";
            this.btnLsvTest.Size = new System.Drawing.Size(75, 23);
            this.btnLsvTest.TabIndex = 6;
            this.btnLsvTest.Text = "List View Test";
            this.btnLsvTest.UseVisualStyleBackColor = true;
            this.btnLsvTest.Click += new System.EventHandler(this.btnLsvTest_Click);
            // 
            // btnMidiToLSV
            // 
            this.btnMidiToLSV.Location = new System.Drawing.Point(356, 150);
            this.btnMidiToLSV.Name = "btnMidiToLSV";
            this.btnMidiToLSV.Size = new System.Drawing.Size(86, 32);
            this.btnMidiToLSV.TabIndex = 7;
            this.btnMidiToLSV.Text = "Midi to List View";
            this.btnMidiToLSV.UseVisualStyleBackColor = true;
            this.btnMidiToLSV.Click += new System.EventHandler(this.btnMidiToLSV_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 715);
            this.Controls.Add(this.btnMidiToLSV);
            this.Controls.Add(this.btnLsvTest);
            this.Controls.Add(this.lsvMidi);
            this.Controls.Add(this.txtMidiFileOutput);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtData);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtMidiFileOutput;
        private System.Windows.Forms.ListView lsvMidi;
        private System.Windows.Forms.Button btnLsvTest;
        private System.Windows.Forms.Button btnMidiToLSV;
    }
}

