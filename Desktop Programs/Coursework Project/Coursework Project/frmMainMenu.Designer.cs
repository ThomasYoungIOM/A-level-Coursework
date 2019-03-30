namespace Coursework_Project {
    partial class frmMainMenu {
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvExercises = new System.Windows.Forms.DataGridView();
            this.lbl1 = new System.Windows.Forms.Label();
            this.cmbInstuments = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLookAtMusic = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnLoadMidi = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLoadedExercise = new System.Windows.Forms.Label();
            this.ExerciseID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Difficulty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instrument = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileExists = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExercises)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvExercises
            // 
            this.dgvExercises.AllowUserToAddRows = false;
            this.dgvExercises.AllowUserToDeleteRows = false;
            this.dgvExercises.AllowUserToResizeRows = false;
            this.dgvExercises.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Roboto", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExercises.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvExercises.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExercises.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ExerciseID,
            this.FileName,
            this.FilePath,
            this.Difficulty,
            this.Instrument,
            this.FileExists});
            this.dgvExercises.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvExercises.Location = new System.Drawing.Point(268, 53);
            this.dgvExercises.MultiSelect = false;
            this.dgvExercises.Name = "dgvExercises";
            this.dgvExercises.ReadOnly = true;
            this.dgvExercises.RowHeadersVisible = false;
            this.dgvExercises.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExercises.Size = new System.Drawing.Size(486, 192);
            this.dgvExercises.TabIndex = 0;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lbl1.Location = new System.Drawing.Point(12, 22);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(196, 25);
            this.lbl1.TabIndex = 7;
            this.lbl1.Text = "Select an Instrument:";
            // 
            // cmbInstuments
            // 
            this.cmbInstuments.FormattingEnabled = true;
            this.cmbInstuments.Items.AddRange(new object[] {
            "All"});
            this.cmbInstuments.Location = new System.Drawing.Point(268, 26);
            this.cmbInstuments.MaxDropDownItems = 20;
            this.cmbInstuments.Name = "cmbInstuments";
            this.cmbInstuments.Size = new System.Drawing.Size(121, 21);
            this.cmbInstuments.TabIndex = 1;
            this.cmbInstuments.SelectedIndexChanged += new System.EventHandler(this.cmbInstuments_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(12, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select an Exercise:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(12, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "What would you like to do?";
            // 
            // btnLookAtMusic
            // 
            this.btnLookAtMusic.Enabled = false;
            this.btnLookAtMusic.Location = new System.Drawing.Point(12, 275);
            this.btnLookAtMusic.Name = "btnLookAtMusic";
            this.btnLookAtMusic.Size = new System.Drawing.Size(196, 47);
            this.btnLookAtMusic.TabIndex = 2;
            this.btnLookAtMusic.Text = "Look At Music";
            this.btnLookAtMusic.UseVisualStyleBackColor = true;
            this.btnLookAtMusic.Click += new System.EventHandler(this.btnLookAtMusic_Click);
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(268, 275);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(242, 47);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Practice and Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(563, 275);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(191, 47);
            this.btnQuit.TabIndex = 6;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.BtnQuit_Click);
            // 
            // btnLoadMidi
            // 
            this.btnLoadMidi.Location = new System.Drawing.Point(17, 86);
            this.btnLoadMidi.Name = "btnLoadMidi";
            this.btnLoadMidi.Size = new System.Drawing.Size(240, 47);
            this.btnLoadMidi.TabIndex = 11;
            this.btnLoadMidi.Text = "Load Exercise";
            this.btnLoadMidi.UseVisualStyleBackColor = true;
            this.btnLoadMidi.Click += new System.EventHandler(this.btnLoadMidi_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.Location = new System.Drawing.Point(12, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(248, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "Currently Loaded Exercise:";
            // 
            // lblLoadedExercise
            // 
            this.lblLoadedExercise.AutoSize = true;
            this.lblLoadedExercise.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblLoadedExercise.Location = new System.Drawing.Point(14, 181);
            this.lblLoadedExercise.Name = "lblLoadedExercise";
            this.lblLoadedExercise.Size = new System.Drawing.Size(0, 20);
            this.lblLoadedExercise.TabIndex = 13;
            // 
            // ExerciseID
            // 
            this.ExerciseID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ExerciseID.DataPropertyName = "ExerciseID";
            this.ExerciseID.HeaderText = "ID";
            this.ExerciseID.Name = "ExerciseID";
            this.ExerciseID.ReadOnly = true;
            this.ExerciseID.Visible = false;
            // 
            // FileName
            // 
            this.FileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileName.DataPropertyName = "FileName";
            this.FileName.HeaderText = "Name";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            // 
            // FilePath
            // 
            this.FilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FilePath.DataPropertyName = "FilePath";
            this.FilePath.HeaderText = "File Path";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            // 
            // Difficulty
            // 
            this.Difficulty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Difficulty.DataPropertyName = "Difficulty";
            this.Difficulty.HeaderText = "Difficulty (1-10)";
            this.Difficulty.Name = "Difficulty";
            this.Difficulty.ReadOnly = true;
            // 
            // Instrument
            // 
            this.Instrument.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Instrument.DataPropertyName = "Instrument";
            this.Instrument.HeaderText = "Instrument";
            this.Instrument.Name = "Instrument";
            this.Instrument.ReadOnly = true;
            // 
            // FileExists
            // 
            this.FileExists.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FileExists.DataPropertyName = "FileExists";
            this.FileExists.HeaderText = "File Exists";
            this.FileExists.Name = "FileExists";
            this.FileExists.ReadOnly = true;
            this.FileExists.Visible = false;
            // 
            // frmMainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 339);
            this.Controls.Add(this.lblLoadedExercise);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnLoadMidi);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnLookAtMusic);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbInstuments);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.dgvExercises);
            this.Name = "frmMainMenu";
            this.Text = "Main Menu";
            this.Load += new System.EventHandler(this.frmMainMenu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExercises)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvExercises;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.ComboBox cmbInstuments;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLookAtMusic;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.Button btnLoadMidi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLoadedExercise;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExerciseID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn Difficulty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instrument;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileExists;
    }
}

