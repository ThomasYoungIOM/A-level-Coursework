namespace Midi_Serial_sender {
    partial class ColourFile {
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
            this.btnColour = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnColour
            // 
            this.btnColour.Location = new System.Drawing.Point(12, 12);
            this.btnColour.Name = "btnColour";
            this.btnColour.Size = new System.Drawing.Size(75, 23);
            this.btnColour.TabIndex = 0;
            this.btnColour.Text = "Colour File";
            this.btnColour.UseVisualStyleBackColor = true;
            this.btnColour.Click += new System.EventHandler(this.btnColour_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "mid";
            this.openFileDialog1.Filter = "Midi Files|*.mid";
            this.openFileDialog1.InitialDirectory = "C:\\Users\\Thomas\\Documents\\Computer Science Coursework\\Midi Files";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 42);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(775, 405);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "Hello, how are your";
            // 
            // ColourFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnColour);
            this.Name = "ColourFile";
            this.Text = "ColourFile";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnColour;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}