namespace Prototype_Project {
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
            System.Windows.Forms.Label lable1;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.ofdOpenMidi = new System.Windows.Forms.OpenFileDialog();
            lable1 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lable1
            // 
            lable1.AutoSize = true;
            lable1.CausesValidation = false;
            lable1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            lable1.Location = new System.Drawing.Point(13, 13);
            lable1.Name = "lable1";
            lable1.Size = new System.Drawing.Size(100, 31);
            lable1.TabIndex = 1;
            lable1.Text = "Step 1:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.CausesValidation = false;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            label1.Location = new System.Drawing.Point(13, 80);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 31);
            label1.TabIndex = 3;
            label1.Text = "Step 2:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.CausesValidation = false;
            label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            label2.Location = new System.Drawing.Point(16, 111);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(112, 17);
            label2.TabIndex = 4;
            label2.Text = "Select Instument";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.CausesValidation = false;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            label3.Location = new System.Drawing.Point(13, 315);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(100, 31);
            label3.TabIndex = 5;
            label3.Text = "Step 3:";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Ch  1: Volin",
            "Ch  2: Chello",
            "Ch  3: Trumpet",
            "Ch  4: Tin Whistle "});
            this.checkedListBox1.Location = new System.Drawing.Point(134, 111);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(271, 154);
            this.checkedListBox1.TabIndex = 0;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnOpenFile.Location = new System.Drawing.Point(119, 13);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(84, 31);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPlay.Location = new System.Drawing.Point(119, 315);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(84, 30);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.Text = "Play!";
            this.btnPlay.UseVisualStyleBackColor = true;
            // 
            // ofdOpenMidi
            // 
            this.ofdOpenMidi.DefaultExt = "mid";
            this.ofdOpenMidi.Filter = "MIDI Files|*.mid";
            this.ofdOpenMidi.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdOpenMidi_FileOk);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 401);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(lable1);
            this.Controls.Add(this.checkedListBox1);
            this.Name = "frmMain";
            this.Text = "Tin Whistle Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.OpenFileDialog ofdOpenMidi;
    }
}

