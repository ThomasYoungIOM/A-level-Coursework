using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;


namespace Coursework_Project {
    public class midiFile {
        #region Exercise Varibles
        string p_instrument;        //Stores the instrument that the exercise is to be played on
        string p_exerciseName;      //Stores the name of the exercise
        int p_exerciseId;           //Stores the exercise ID
        byte p_difficulty;          //Stores the difficulty of the exercise
        #endregion

        #region Midi varibles
        List<Note> p_listOfNotes;   //List of notes
        ushort p_devision;         //Stores the tempo of the peice. 
        uint p_tempo;              //Stores the tempo of the peice in the format tt tt tt where the value is the number of micro seconds per Crochet. Default is 500000 (120bpm)
        uint p_timeSig;            //Time sig. Format: nn dd cc dd     nn/2^dd     nn - Numerator  dd - denomiator     cc - Clock ticks per metronome tick     bb - Number of 1/32 notes per 24 MIDI clocks (8 normally)
        ushort p_keySig;           //key  sig. Format: sf mi           sf - Number of Sharps or flats |     mi - Major (0) or minor (1)
        public List<Bitmap> p_listOfStaves; //@@@@@@@
        #endregion

        /// <summary>
        /// Instantiats the midi file class with all of the important values that are required
        /// </summary>
        /// <param name="timeSig">The time signalture of the peice</param>
        /// <param name="devision">The tempo of the peice</param>
        /// <param name="keySig">The key sig of the peice</param>
        /// <param name="listOfNotes">The list of notes that makes up the peice</param>
        public midiFile( ushort? devision, uint? tempo, uint? timeSig, ushort? keySig, List<Note> listOfNotes, string instrument, databaseInterface databaseInterface, out bool error, out string errorString) {
            //Assign the inputted data to the private varibles
            //If the input is null, then assign the default midi values to everything
            p_devision = devision ?? (ushort)(120);
            p_tempo = tempo ?? (uint)(500000);
            p_timeSig = timeSig ?? (uint)(0x04022408);
            p_keySig = keySig ?? (ushort)(0x0000);
            p_listOfNotes = listOfNotes;
            p_instrument = instrument;

            //Generate the list of bitmaps that is the list of staves
            error = GenerateStaves(databaseInterface, out errorString);
        }

        #region Get Sets

        #region midiVariables
        public List<Note> listOfNotes {
            get {
                return p_listOfNotes;
            }
        }
        //@@
        public ushort devision {
            get {
                return p_devision; // ?? (ushort)(120);         //If the value is null, then return the default tempo which is 120 bpm
            }
        }


        public uint tempo {
            get {
                return p_tempo;  //  ?? (uint)(500000);       //If the value is null, then return the default value of 500000 (120bpm)
            }
        }

        public uint timeSig {
            get {
                return p_timeSig; // ?? (uint)(0x04022408);     //If the value is null, then return the default MIDI time signature
            }
        }


        public ushort keySig {
            get {
                return p_keySig; // ?? (ushort)(0x0000);        //If the value is null, then return the key sig to be C major
            }
        }
        #endregion

        #region Exercise Varibles
        public string Instrument {
            get {
                return p_instrument;
            }
        }
        #endregion


        #endregion

        #region Methods


        /// <summary>
        /// Generates the list of staves that have the images for the display and assignes it to p_listOfStaves
        /// </summary>
        /// <param name="databaseConnection">The database connection to use to get the note data out of the database to allow the notes to be drawn in the right place</param>
        /// <param name="errorString">Outs any errors that may have occured</param>
        /// <returns>whether there was an error or not</returns>
        public bool GenerateStaves(databaseInterface databaseConnection, out string errorString) {
            errorString = null;             //Stores any errors that can be passed back to the enclosing method
            List<Bitmap> listOfStaves = new List<Bitmap>();     //this will store the list of generated bitmaps that will be u
            List<Bitmap> listOfFingering = new List<Bitmap>();  //This will store the lines of generated bitmaps that will display the fingering to the user
            const int staveValue = 45;
            const int bottomYPos = 365;     //Stores the y pos of the bottom line of the stave from which all notes will be drawn above
            const int cleffOffset = 300;
            int[] barOffsets = { 515, 1863, 3211, 4551 };        //Stores the x position of the start of the bars
            int ticksPerBar = (int)(p_devision) * (int)(p_timeSig >> 24);  //Ticks per note times the number of notes in a bar
            int currentBarTickCount;        //Stores the current number of ticks that have been found in the list of notes
            byte currentNotesInBar = 0;          //Stores the number of notes that will need to be displayed in the current bar. Each bar is 1348px wide and starts 515px after the start of the last bar
            int notesToDrawInBar = 0;        //Stores the number of actually visable notes that will be drawn
            int currentNoteIndex = 0;                //Sores the index of the current note being processed
            int currentDrawingNote = 0;
            byte currentBar = 0;        //Stores the number bar that is currently being drawn
            const int barWidth = 1348;  //The width of one bar
            byte staveLocation = 0;

            Dictionary<byte, byte> noteStaveLocation;       //Stores the note value as the key and the location on the stave (0 - middle c)
            SqlCommand sqlCommandToRun = new SqlCommand();

            Bitmap bitmapToDraw = new Bitmap(Properties.Resources.NotFound.Width, Properties.Resources.NotFound.Height); //= Properties.Resources.NotFound;        //Saves the image of the right length note that will be drawn

            int firstNoteIndex = 0;     //Stores the index of the first note in the bar
            //int staveStartOffsetX = 300;

            Bitmap currentStaff = Properties.Resources.StaffWithBars;
            Graphics currentStaffGraphics = Graphics.FromImage(currentStaff);


            sqlCommandToRun.CommandText = "SELECT Note, StaveLocation FROM Notes WHERE Instrument = @instrument";        //Sql Commmand that will select all the exercises that use the right instrument
            sqlCommandToRun.Parameters.AddWithValue("@instrument", p_instrument);      //If the selected item is "All", then set the parameter to *, else set it to the inputted string

            //Populate the noteStave location dictionary
            try {

                //Creates a distionary from the datatable returned from the SQL query
                //                                                                                               ↓Loops throug the table, transmutes the type to a byte which can then be converted to a dicatonary
                noteStaveLocation = databaseConnection.executeQuery(sqlCommandToRun).AsEnumerable().ToDictionary(currentRow => currentRow.Field<byte>(0), currentRow => currentRow.Field<byte>(1));

            } catch (Exception) {
                throw;
            }




            //Draw time signature
            //nn dd cc dd     nn / 2 ^ dd     nn - Numerator  dd - denomiator     cc - Clock ticks per metronome tick     bb - Number of 1 / 32 notes per 24 MIDI clocks(8 normally)
            //Numerator = (timeSig >> 24)
            //Denominat = (2^(midiFile.timeSig >> 16) & 0xFF)
            currentStaffGraphics.DrawString((p_timeSig >> 24).ToString(), new Font("Arial", 175), Brushes.Black, cleffOffset, 100);
            currentStaffGraphics.DrawString(Math.Pow(2, (p_timeSig >> 16) & 0xFF).ToString(), new Font("Arial", 175), Brushes.Black, cleffOffset, 300);





            //Loop through all of the staves
            while (p_listOfNotes.Count > currentNoteIndex + 1) {

                currentBar = 0;

                //Loop through all of the bars
                while (currentBar < 4 && (p_listOfNotes.Count > currentNoteIndex + 1)) {



                    //Get a list of all of the notes that will be displayed in the current bar
                    firstNoteIndex = firstNoteIndex + currentNotesInBar + 1 < p_listOfNotes.Count ? firstNoteIndex + currentNotesInBar : p_listOfNotes.Count - 1;
                    currentBarTickCount = 0;
                    currentNotesInBar = 0;
                    notesToDrawInBar = 0;
                    currentDrawingNote = 0;
                    bitmapToDraw = Properties.Resources.NotFound;

                    do {
                        //Only count the note if it is long enough to be counted
                        if (p_listOfNotes[currentNoteIndex].length / p_devision > 0.25f) {
                            currentBarTickCount += p_listOfNotes[currentNoteIndex].length;
                            notesToDrawInBar++;
                        }
                        currentNotesInBar++;
                        currentNoteIndex++;

                    } while ((currentBarTickCount <= ticksPerBar - p_listOfNotes[currentNoteIndex].length) && (p_listOfNotes.Count > currentNoteIndex + 1));



                    //Display those notes on the stave
                    for (int i = firstNoteIndex; i < firstNoteIndex + currentNotesInBar; i++) {


                        //Only draw the note if it is not too short
                        if ((float)p_listOfNotes[i].length / (float)p_devision >= 0.25) {




                            //Find out if the note is a rest of not
                            if (p_listOfNotes[i].noteNum != 0) {

                                //Find out what image to draw
                                switch ((float)p_listOfNotes[i].length / p_devision) {

                                    //Draw a Quaver
                                    case float len when (len > 0.25 && len <= 0.75):
                                        bitmapToDraw = Properties.Resources.EighthNote;
                                        break;

                                    //Draw a Crotchet
                                    case float len when (len > 0.75 && len <= 1.25):
                                        bitmapToDraw = Properties.Resources.QuarterNote;
                                        break;

                                    //Draw a Dotted crochet
                                    case float len when (len > 0.25 && len <= 1.75):
                                        bitmapToDraw = Properties.Resources.DottedQuarterNote;
                                        break;

                                    //Draw a Minum
                                    case float len when (len > 1.75 && len <= 2.5):
                                        bitmapToDraw = Properties.Resources.HalfNote;
                                        break;

                                    //Draw a Dotted minum
                                    case float len when (len > 2.5 && len <= 3.5):
                                        bitmapToDraw = Properties.Resources.DottedHalfNote;
                                        break;

                                    //Draw a Whole note
                                    case float len when (len > 3.5 && len <= 5.0):
                                        bitmapToDraw = Properties.Resources.WholeNote;
                                        break;

                                    //Draw a dotted whole note
                                    case float len when (len > 5.0 && len <= 7.0):
                                        bitmapToDraw = Properties.Resources.DottedWholeNote;
                                        break;


                                    default:
                                        //Note length that cannot be drawn
                                        bitmapToDraw = Properties.Resources.NotFound;
                                        break;
                                }

                                //Draw the note at the right x-coord and the right y-coord



                                //Gets the stave location from the dictionary
                                if (!noteStaveLocation.TryGetValue(p_listOfNotes[i].noteNum, out staveLocation)) {
                                    bitmapToDraw = Properties.Resources.NotFound;
                                }

                            } else {        //Else, note is a rest and a rest should be drawn

                            }
                            currentStaffGraphics.DrawImage(bitmapToDraw, barWidth / (notesToDrawInBar != 0 ? notesToDrawInBar : 1) * (currentDrawingNote) + barOffsets[currentBar], bottomYPos - staveLocation * staveValue);
                            currentDrawingNote++;
                        }
                    }
                    currentBar++;
                }
                listOfStaves.Add(currentStaff);

                currentStaff = Properties.Resources.StaffWithBars;
                currentStaffGraphics = Graphics.FromImage(currentStaff);

            }
        




            p_listOfStaves = listOfStaves;
            return true;
            

        }


        #endregion


    }


    /// <summary>
    /// Struct to store all the neccerary information about a note event. (The Absolute time it starts, the note to be played and the length)
    /// </summary>
    public struct Note {
        //delta Time
        public readonly int absoluteTime;
        //Note
        public readonly byte noteNum;
        //Length
        public 
            readonly int length;

        public Note(int inAbsoluteTime, byte inNote, int inLength) {
            absoluteTime = inAbsoluteTime;
            noteNum = inNote;
            length = inLength;
        }
    }
}
