using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Coursework_Project {
    public partial class frmViewMusic : Form {
        const string dbConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Thomas\source\repos\A-level Coursework\Desktop Programs\Coursework Project\CourseworkDatabase.mdf';Integrated Security=True;Connect Timeout=30";

        static Bitmap[] noteBitmaps;                       //Stores the image of the note with the duration
        static Dictionary<byte, Bitmap> fingeringBitmaps;  //Stores the fingering charts for that instrument
        midiFile loadedMidiFile;

        

        #region Form Event Triggered Subs
        public frmViewMusic(midiFile inputFile) {
            string errorString;
            Bitmap generatedScore;

            loadedMidiFile = inputFile;

            InitializeComponent();


            //Load bitmaps
            noteBitmaps = null;



            loadedMidiFile.GenerateMusicPage(picDisplay.Width, picDisplay.Height, 0, false, false, out Bitmap pageToDisplay,out int nextLine, out errorString);




            picDisplay.Image = pageToDisplay;
            


        }

        #endregion

        


    }
}
