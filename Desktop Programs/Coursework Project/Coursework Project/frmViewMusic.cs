using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coursework_Project {
    public partial class frmViewMusic : Form {


        static Bitmap[] noteBitmaps;                       //Stores the image of the note with the duration
        static Dictionary<byte, Bitmap> fingeringBitmaps;  //Stores the fingering charts for that instrument


        

        #region Form Event Triggered Subs
        public frmViewMusic(midiFile inputFile) {
            string errorString;
            Bitmap generatedScore;

            InitializeComponent();

            //Load bitmaps
            noteBitmaps = null;
            if (!GenerateBitmap(inputFile, out generatedScore, out errorString))
                MessageBox.Show(errorString);
                
        }

        #endregion

        #region Called Subs



        private bool GenerateBitmap(midiFile midiFile, out Bitmap generatedScore, out string errorString) {
            generatedScore = null;

            int noteIndex = 0;

            //Draw each line and save to array
            GenerateStaff(midiFile,ref noteIndex, out errorString);
            
            //Loop through array and draw each bitmap onto the main image
            


            return true;
        }
        


        private bool GenerateStaff(midiFile midiFile,ref int noteIndex, out string errorString) {
            errorString = "";

            Bitmap currentStaff = Properties.Resources.Staff;   //Stores the generated bitmap
            Graphics currentStaffGraphics = Graphics.FromImage(currentStaff);   //The graphics that will store the image as it is being generated 

            const int startOffsetX = 300;    //Stores the X offset from the trebble cleff so it can start drawing
            const int noteOffsetX = 200;      //Stores the X gap between each drawn note
            const int offsetY = 10;
            int currentOffsetX = startOffsetX;    //Stores the current X offset
            float currentNoteLength;         //Stores the length of the current note reletive to the length of 1 crochet
            Point currentNoteLocation;


            //Draw time signature
            //nn dd cc dd     nn / 2 ^ dd     nn - Numerator  dd - denomiator     cc - Clock ticks per metronome tick     bb - Number of 1 / 32 notes per 24 MIDI clocks(8 normally)
            //Numerator = (timeSig >> 24)
            //Denominat = (2^(midiFile.timeSig >> 16) & 0xFF)
            currentStaffGraphics.DrawString((midiFile.timeSig >> 24).ToString(), new Font("Arial",175), Brushes.Black,startOffsetX, 100);
            currentStaffGraphics.DrawString(Math.Pow(2,(midiFile.timeSig >> 16) & 0xFF).ToString(), new Font("Arial", 175), Brushes.Black, startOffsetX, 300);

            currentOffsetX += 250;


            #region Testing drawing
            //Draw note
            currentStaffGraphics.DrawImage(Properties.Resources.WholeNote, new Rectangle(currentOffsetX, 200, 107, 300));
            currentOffsetX += noteOffsetX;

            currentStaffGraphics.DrawImage(Properties.Resources.HalfNote, new Rectangle(currentOffsetX, 200, 107, 300));
            currentOffsetX += noteOffsetX;

            currentStaffGraphics.DrawImage(Properties.Resources.QuarterNote, new Rectangle(currentOffsetX, 200, 107, 300));
            currentOffsetX += noteOffsetX;

            currentStaffGraphics.DrawImage(Properties.Resources.EighthNote, new Rectangle(currentOffsetX, 200, 162, 300));
            currentOffsetX += noteOffsetX;
            #endregion

            
            //Get note length
            //The division is the number of ticks per crochet. This means that work out how many crochets long it is, you take the length of the note in ticks and devide by the division of the music.
            currentNoteLength = midiFile.listOfNotes[noteIndex].length/midiFile.devision;

            //Work out what coordinates to draw the note at
            


            if (currentNoteLength < 0.75) {
                //Draw Quaver
            } else if (currentNoteLength < 1.25) {
                //Draw Crochet
            } else if (currentNoteLength < 1.75) {
                //Draw Dotted Crochet
            } else if(currentNoteLength < 2.5) {
                //Draw Minim
            } else if(currentNoteLength < 3.5) {
                //Draw Dotted Minim
            } else if (currentNoteLength < 5) {
                //Draw Semibreve
            } else if (currentNoteLength < 6) {
                //Draw dotted semibreve
            }


            //draw note image at pitch

            //Draw bar (after the correct number of beats have passed)

            //end the line


            return false;
        }

        private void CloseForm() {
            
        }


        #endregion
    }
}
