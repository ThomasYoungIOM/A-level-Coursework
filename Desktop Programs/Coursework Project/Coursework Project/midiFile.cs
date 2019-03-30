
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.IO;


namespace Coursework_Project {
    public class midiFile {
        #region Exercise Varibles
        string p_instrument;        //Stores the instrument that the exercise is to be played on
        string p_exerciseName;      //Stores the name of the exercise
        int p_exerciseId;           //Stores the exercise ID
        byte p_difficulty;          //Stores the difficulty of the exercise
        #endregion

        #region Midi Varibles
        readonly List<Note> p_listOfNotes;   //List of notes
        readonly ushort p_devision;         //Ticks per quarter note
        readonly uint p_tempo;              //Stores the tempo of the peice in the format tt tt tt where the value is the number of micro seconds per Crochet. Default is 500000 (120bpm)
        readonly uint p_timeSig;            //Time sig. Format: nn dd cc dd     nn/2^dd     nn - Numerator  dd - denomiator     cc - Clock ticks per metronome tick     bb - Number of 1/32 notes per 24 MIDI clocks (8 normally)
        readonly ushort p_keySig;           //key  sig. Format: sf mi           sf - Number of Sharps or flats |     mi - Major (0) or minor (1)
        #endregion

        #region Generated Varibles
        //public List<Bitmap> p_musicPages; @@ try to not have this
        private List<Bitmap> p_listOfStaves; //@@@@@@@
        private List<Bitmap> p_listOfFingeringBitmaps;
        private List<Bitmap> p_listOfRightNotes;     //Stores the bitmaps which show what the user should've played
        private List<Bitmap> p_listOfPlayedBitmaps; //Stores the bitmaps which show what the user has played 
        //public Bitmap p_rightNoteBitmap;
        private int p_lowestBin;      //Stores the number lowest bin number that is displayed on the example bitmap of what the user should've played
        private int p_highestBin;     //Stores the highest bin number that is displayed on the example bitmap of what the user should've played
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

            //Set up the Generate all the things that will be required throughout the program
            //This would be the bitmap lists 
            //Generate the list of bitmaps that is the list of staves
            error = !GenerateStaves(databaseInterface, out errorString);
            if(error)
                return;

            //Generate the example bitmap that will show what the correct image should be 
            error = !GenerateExampleBitmap(databaseInterface, out errorString);
            if (error)
                return;


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

        #region Generated Varibles
        //Reutrns the number Of staves
        public int numberOfStaves {
            get {
                return p_listOfStaves.Count;
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
            List<Bitmap> listOfStaves = new List<Bitmap>();     //this will store the list of generated bitmaps that will be use
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

            DataTable inputDatatable;       //Data table that stores the stuff returned for the database cuz devising a LINQ expression for dealing with converting byte streams to bitmaps would be wayyy too annoying and uneccersary. 
            //MemoryStream bitmapMemStream;   //Memory stream that allows me to convert from the byte array stored in the database to the bitmap images (Don't use "Using" with this on cuz as long as the bitmap needs to exsists, the stream it cam from will need to exist to. Probably not neccersary since this is a memore stream not a binary stream)

            Dictionary<byte, byte> noteStaveLocation;       //Stores the note value as the key and the location on the stave (0 - middle c)
            Dictionary<byte, Bitmap> noteFingeringDict = new Dictionary<byte, Bitmap>();     //Stores the note value as the key and the image of the fingering of that note as the value

            SqlCommand sqlCommandToRun = new SqlCommand();

            Bitmap bitmapToDraw = new Bitmap(Properties.Resources.NotFound.Width, Properties.Resources.NotFound.Height); //= Properties.Resources.NotFound;        //Saves the image of the right length note that will be drawn
            Bitmap fingeringToDraw = new Bitmap(Properties.Resources.NotFound.Width, Properties.Resources.NotFound.Height);     //Used to save the image of the fngering for that particular note

            int firstNoteIndex = 0;     //Stores the index of the first note in the bar
            //int staveStartOffsetX = 300;

            Bitmap currentStaff = Properties.Resources.StaffWithBars;
            Graphics currentStaffGraphics = Graphics.FromImage(currentStaff);

            Bitmap currentGeneratedFingering = new Bitmap(currentStaff.Width, currentStaff.Height);
            Graphics currentGeneratedFingeringGraphics = Graphics.FromImage(currentGeneratedFingering);

            sqlCommandToRun.CommandText = "SELECT Note, StaveLocation FROM Notes WHERE Instrument = @instrument";        //Sql Commmand that will select all the exercises that use the right instrument
            sqlCommandToRun.Parameters.AddWithValue("@instrument", p_instrument);      //If the selected item is "All", then set the parameter to *, else set it to the inputted string

            //Populate the noteStave location dictionary
            try {

                //Creates a distionary from the datatable returned from the SQL query
                //                                                                                               ↓Loops throug the table, transmutes the type to a byte which can then be converted to a dicatonary
                noteStaveLocation = databaseConnection.executeQuery(sqlCommandToRun).AsEnumerable().ToDictionary(currentRow => currentRow.Field<byte>("Note"), currentRow => currentRow.Field<byte>("StaveLocation"));




                //Creates a dictionary that stores the note values and the fingering images that will be used to drawing the fingering bitmaps that will be displayed beneath the note images
                sqlCommandToRun = new SqlCommand("SELECT Note, FingeringDrawing FROM Notes WHERE Instrument = @instrument");
                sqlCommandToRun.Parameters.AddWithValue("@instrument", p_instrument);

                inputDatatable = databaseConnection.executeQuery(sqlCommandToRun);


                //Iterate through the returned datatable from the database and get the image and add that to the dictionary
                foreach (DataRow row in inputDatatable.Rows) {
                    noteFingeringDict.Add(row.Field<byte>("Note"), new Bitmap(new MemoryStream(row.Field<byte[]>("FingeringDrawing"))));      //For the image bit: Get the byte array from the datatable, then create a memory stream from this byte array, then create a bitmap from this memorey stream and then throw that into the dictionary
                }


            } catch (Exception) {
                errorString = "Problem acsessing the database";
                return false;
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
                while (currentBar < 4 && (p_listOfNotes.Count > currentNoteIndex + 1 )) {



                    //Get a list of all of the notes that will be displayed in the current bar
                    firstNoteIndex = firstNoteIndex + currentNotesInBar + 1 < p_listOfNotes.Count ? firstNoteIndex + currentNotesInBar : p_listOfNotes.Count - 1;
                    currentBarTickCount = 0;
                    currentNotesInBar = 0;
                    notesToDrawInBar = 0;
                    currentDrawingNote = 0;
                    bitmapToDraw = Properties.Resources.NotFound;

                    //Loop untill there are the right number of beats in the bar
                    do {
                        //Only count the note if it is long enough to be counted
                        if ((float)p_listOfNotes[currentNoteIndex].length / (float)p_devision >= 0.20f) {
                            currentBarTickCount += p_listOfNotes[currentNoteIndex].length;
                            notesToDrawInBar++;
                        }
                        currentNotesInBar++;
                        currentNoteIndex++;

                    } while ((p_listOfNotes.Count > currentNoteIndex) && (currentBarTickCount <= ticksPerBar - p_listOfNotes[currentNoteIndex].length));



                    //Display those notes on the stave
                    for (int i = firstNoteIndex; i < firstNoteIndex + currentNotesInBar; i++) {


                        //Only draw the note if it is not too short
                        if ((float)p_listOfNotes[i].length / (float)p_devision >= 0.20) {


                            //Find out if the note is a rest of not
                            if (p_listOfNotes[i].noteNum != 0) {

                                //Find out what image to draw
                                switch ((float)p_listOfNotes[i].length / p_devision) {

                                    //Draw a Quaver
                                    case float len when (len > 0.20 && len <= 0.75):
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


                                //Gets the stave location from the dictionary
                                if (!noteStaveLocation.TryGetValue(p_listOfNotes[i].noteNum, out staveLocation)) {
                                    staveLocation = 5;  //Draw all the X's in the same location
                                    bitmapToDraw = Properties.Resources.NotFound;       //show that the image cannot be found
                                }

                                //If the fingering diagram cannot be found in the dictionary, then display a note found in it's place
                                if (!noteFingeringDict.TryGetValue(p_listOfNotes[i].noteNum, out fingeringToDraw)) {
                                    fingeringToDraw = Properties.Resources.NotFound;
                                }

                            } else {        //Else, note is a rest and a rest should be drawn

                            }
                            //Draw the right note in the right place
                            currentStaffGraphics.DrawImage(bitmapToDraw, barWidth / (notesToDrawInBar != 0 ? notesToDrawInBar : 1) * (currentDrawingNote) + barOffsets[currentBar], bottomYPos - staveLocation * staveValue);

                            //Draw the fingering for the current note in the same place on another bitmap for use later
                            currentGeneratedFingeringGraphics.DrawImage(fingeringToDraw, barWidth / (notesToDrawInBar != 0 ? notesToDrawInBar : 1) * (currentDrawingNote) + barOffsets[currentBar], 0);

                            currentDrawingNote++;
                        }
                    }
                    currentBar++;
                }



                listOfStaves.Add(currentStaff);
                listOfFingering.Add(currentGeneratedFingering);


                currentStaff = Properties.Resources.StaffWithBars;
                currentStaffGraphics = Graphics.FromImage(currentStaff);

            }
        




            p_listOfStaves = listOfStaves;
            p_listOfFingeringBitmaps = listOfFingering;
            return true;
            

        }

        /* 
         * --Initilization--
         * Calculate how many seconds are displayed in one line of music (Devision * Beats per bar * num of bars)
         * Calculate the number of pixels per second (width of one staff / seconds in one staff)
         * Calculate the width of the bitmap that will be required (Pixels Per Second * number of seconds)
         * 
         * --Drawing the Notes that should've been played on--
         * Calculate the width of the box to draw (Note length * devision * Pixels per second)
         * Calucalte hieght to draw the box at (The line is split up into the range of highest bin number to lowest bin number using a scale factor = (height / Number of bins) and the lowest bin number box drawn at the botton)
         * 
         * 
         * --Drawing the notes that were actually played on--
         * 
         * *******************************************************
         * How to do the drawing bit: 
         * 
         */

        private bool GenerateExampleBitmap(databaseInterface databaseConnection, out string errorString) {
            //Set up all of the varibles that will be used
            int ticksPerLine = p_devision * (int)(p_timeSig >> 24) * 4;                 //Number of MIDI ticks that one line of music represents (Ticks per quarter note * Quarter notes per bar * number of bar in line)
            //float secondsPerLine = (int)(p_timeSig >> 24) * (p_tempo/1000000) * 4;        //Gets the number of seconds that 1 line of music repreents     (Qauter note per bar * (Seconds per quater note) * number of bars in each line)
            //float pixelsPerSecond = secondsPerLine / p_listOfStaves[0].Width;             //Gets the number of pixels that 1 seconds takes up (Seconds per line / pixels per line)
            float pixelsPerTick = (float)p_listOfStaves[0].Width / ticksPerLine;                  //Gets the number of pixels represented by 1 tick
            int widthOfBitmap = (int)(pixelsPerTick * (p_listOfNotes.Last().absoluteTime + p_listOfNotes.Last().length));     //Gets the width the generated bitmap would need to be store the entire length of the midiFile 
            Dictionary<byte, int> noteFftLocations = new Dictionary<byte, int>();
            int lowestBinNum;      //Stores the lowest frequncy bin that will be displayed as correct
            int largestBinNum;     //Stores the highest frequency bin that will be displayed as correct
            int binHeight;         //Stores the number of vertile pixels that are dedicated to the specific fft bins/pitchest
            int currentNoteFFTPos;  //Stores the FFT pos of a note collected from the dictionary
            List<Bitmap> outListOfExamples = new List<Bitmap>();

            SqlCommand sqlCommandToRun = new SqlCommand();

            Bitmap currentLine = new Bitmap(p_listOfStaves[0].Width, p_listOfStaves[0].Height);
            Graphics currentLineGraphics = Graphics.FromImage(currentLine);

            Bitmap generatedExample;      //Bitmap to store the generated image
            Graphics generatedExampleGraphics;  //Graphics that will handle all the drawing

            errorString = "";

            //Get the dictionary of fft locations
            sqlCommandToRun.CommandText = "SELECT Note, BinNum FROM Notes WHERE Instrument = @instrument";           //Sql Commmand that will select all the exercises that use the right instrument
            sqlCommandToRun.Parameters.AddWithValue("@instrument", p_instrument);                                           //If the selected item is "All", then set the parameter to *, else set it to the inputted string


            //Populate the noteStave location dictionary
            try {

                //Creates a distionary from the datatable returned from the SQL query
                //                                                                                               ↓Loops throug the table, transmutes the type to a byte which can then be converted to a dicatonary
                noteFftLocations = databaseConnection.executeQuery(sqlCommandToRun).AsEnumerable().ToDictionary(currentRow => currentRow.Field<byte>(0), currentRow => currentRow.Field<int>(1));

            } catch (Exception) {
                errorString = "Could not retreive note FFT positions from database";
                return false;
            }

           


            p_lowestBin = lowestBinNum = noteFftLocations.First().Value - 2;  //The lowest bin number that will be displayed. The +-2 is to give clearance above and below the image to allow for the bitmaps that the user played to be displayed
            p_highestBin = largestBinNum = noteFftLocations.Last().Value + 2;  //The highest bin number that will be displayed

            binHeight = p_listOfStaves[0].Height/(largestBinNum - lowestBinNum);    //Gets the height in pixels that will be dedicated to each bin
            


            generatedExample = new Bitmap(widthOfBitmap, p_listOfStaves[0].Height);

            using (generatedExampleGraphics = Graphics.FromImage(generatedExample)) { 

                //Loop through all of the notes that will be drawn
                foreach(Note currentNote in p_listOfNotes) {
                    //only draw anything If the note is not a rest and the note is longer than 0.25 beats
                    if ((float)currentNote.length / p_devision > 0.25f) {

                        //Make sure that the 
                        if (noteFftLocations.TryGetValue(currentNote.noteNum, out currentNoteFFTPos)) {
                            
                            //calculate the x position of the box that needs to be drawn (pixels Per tick * absolute time value)
                            //Get the bin location of the note from the dictionary and convert to a height(
                            //Calculate the width of the box that needs to be drawn (pixelsPerTick * Number of ticks)
                            //Draw the box

                            generatedExampleGraphics.FillRectangle(Brushes.Green, pixelsPerTick * currentNote.absoluteTime, generatedExample.Height - binHeight * (currentNoteFFTPos - lowestBinNum), pixelsPerTick * currentNote.length, binHeight);
                        } else {
                            //Could not find the FFt location so draw an X to show somthing went wrong
                            generatedExampleGraphics.DrawImage(Properties.Resources.NotFound, pixelsPerTick * currentNote.absoluteTime, 0);     
                        }

                    }
                }


            }



            //Split long bitmap into sections
            for (int currentXPos = 0; currentXPos <= generatedExample.Width; currentXPos += p_listOfStaves[0].Width) {
                currentLine = new Bitmap(currentLine.Width, currentLine.Height);            //Clear the bitmap after each use
                currentLineGraphics = Graphics.FromImage(currentLine);

                currentLineGraphics.DrawImage(generatedExample, -currentXPos, 0);           //get the section of the l o n g image that will be the current line
                outListOfExamples.Add(new Bitmap(currentLine));                             //Add the current line to the array
            }


            p_listOfRightNotes = outListOfExamples;


          return true;
        }


     /*   /// <summary>
        /// Generates a page of music to display on the screen
        /// </summary>
        /// <param name="width">The width of the image to display</param>
        /// <param name="height">The height of the image to display</param>
        /// <param name="startLineNum">The index of the first line to display</param>
        /// <param name="drawFingering">Weather the fingering should be drawn below each line</param>
        /// <param name="lastLineNum">Outs the index of the last line that was displayed</param>
        /// <param name="lastLine">Outs whether the generated page contains the last line in the list</param>
        /// <param name="errorString">OUts any errors that were encounted whilst running</param>
        /// <returns>Whetheer the sub was sucessful</returns>
        ///  */


        /// <summary>
        /// Generates a page of music to be displayed to the user
        /// </summary>
        /// <param name="inputWidth">The width of the image that will be returned</param>
        /// <param name="inputHeight">The height of the image that will be returned</param>
        /// <param name="startLineNum">The index of the line to start drawing</param>
        /// <param name="drawFingering">Whether to draw the fingering diagrams</param>
        /// <param name="drawPlayedNotes">Whether to draw the example and the played bitmaps</param>
        /// <param name="generatedPage">Outs the generated bitmap</param>
        /// <param name="nextLineNumber">Outs the index of first line on the NEXT page</param>
        /// <param name="errorString">Outs any errors that were encounted</param>
        /// <returns>Wheather it was sucessful</returns>
        public bool GenerateMusicPage(int inputWidth, int inputHeight, int startLineNum, bool drawFingering, bool drawPlayedNotes, out Bitmap generatedPage, out int nextLineNumber, out string errorString) {
            errorString = "";//@@
            
            
            generatedPage = new Bitmap(inputWidth, inputHeight);        //Create the output image that will be returned

            using (Bitmap fullSizePage = new Bitmap(p_listOfStaves[0].Width, p_listOfStaves[0].Width / generatedPage.Width * generatedPage.Height))     //Generate new bitmap at the full scaling that is used by the resuaerses with the same aspect ratio as the desired output so it can be sacaled down once everything has been written to it.      
            using (Graphics fullsizePageGraphics = Graphics.FromImage(fullSizePage)) {

                int currentYPos = 0;        //Stores the current Y pos that it will draw the next thing at
                int currentStave = 0;
                int nextLineHeight = 0;         //Used to store the calculated height of the next line

                nextLineHeight = getLineHeight(drawFingering, drawPlayedNotes, currentStave);

                //Keep drawing lines until there is not enough space on the page for the next line OR they've run out of lines
                while (fullSizePage.Height >= nextLineHeight + currentYPos
                    && p_listOfStaves.Count > currentStave) {

                    fullsizePageGraphics.DrawImage(p_listOfStaves[currentStave], 0, currentYPos);       //Draw the current line
                    currentYPos += p_listOfStaves[currentStave].Height;     //Update the y pos

                    //Only draw the fingering if it's needed and if there is something to draw
                    if (drawFingering && p_listOfFingeringBitmaps.Count > currentStave) {
                        fullsizePageGraphics.DrawImage(p_listOfFingeringBitmaps[currentStave], 0, currentYPos);
                        currentYPos += p_listOfFingeringBitmaps[currentStave].Height;
                    }

                    //Only draw the played notes it's required and if there are valid bitmaps to draw
                    if (p_listOfPlayedBitmaps != null && drawPlayedNotes && p_listOfRightNotes.Count > currentStave && p_listOfPlayedBitmaps.Count > currentStave) {
                        fullsizePageGraphics.DrawImage(p_listOfRightNotes[currentStave], 0, currentYPos);
                        fullsizePageGraphics.DrawImage(p_listOfPlayedBitmaps[currentStave], 0, currentYPos);
                        currentYPos += p_listOfRightNotes[currentStave].Height;
                    }

                    currentStave++;
                }


                nextLineNumber = currentStave;     //The index of the next line to be drawn will be the same as the current stave
                generatedPage = new Bitmap(fullSizePage, inputHeight, inputHeight);   //Scale the unscaled image down the the image that is wanted for the output
            }
            return true;
        }


        /// <summary>
        /// Gets the height of the next line to be displayed in pixels
        /// </summary>
        /// <param name="drawFingering">Whether fingering should be drawn</param>
        /// <param name="drawPlayedNotes">Weather the played notes should be drawn</param>
        /// <param name="staveIndex">The index of the stave to get the height of</param>
        /// <returns>The hieght of the next stave in px</returns>
        private int getLineHeight(bool drawFingering, bool drawPlayedNotes, int staveIndex) {
            int nextLineHeight = 0;

            //If there are actually any staves to draw
            if (p_listOfStaves.Count > staveIndex + 1) {
                nextLineHeight += p_listOfStaves[staveIndex].Height;

                if (drawFingering && p_listOfFingeringBitmaps.Count > staveIndex + 1) {
                    nextLineHeight += p_listOfFingeringBitmaps[staveIndex].Height;
                }

                if (drawPlayedNotes && p_listOfRightNotes.Count > staveIndex + 1) {
                    nextLineHeight += p_listOfFingeringBitmaps[staveIndex].Height;
                }

            }

            return nextLineHeight;
        }


        /// <summary>
        /// Takes a list of notes, the list of weather audio was detected at a set time and generates a graphical display of what was played which is saved in the class for generating the pages later
        /// </summary>
        /// <param name="audioDetected">The bool list that stores when audio was being played at any time</param>
        /// <param name="audioBins">The int list that stores what what bins were found where</param>
        /// <param name="errorString">Outs any errors that may have occured</param>
        /// <returns>Wheather the sub was successful or not</returns>
        public bool GeneratePlayedBitmaps(List<bool> audioDetected, List<int> audioBins, out string errorString) {
            float secondsPerLine = (float)(p_timeSig >> 24) * p_tempo * 4 / 1000000;        //Gets the number of seconds that 1 line of music repreents     (Qauter note per bar * (Seconds per quater note) * number of bars in each line)
            float pixelsPerSecond = p_listOfStaves[0].Width / secondsPerLine;             //Gets the number of pixels that 1 seconds takes up (Seconds per line / pixels per line)
            errorString = "";

            Dictionary<byte, int> noteFftLocations = new Dictionary<byte, int>();
            int lowestBinNum = p_lowestBin;      //Stores the lowest frequncy bin that will be displayed as correct
            int largestBinNum = p_highestBin;     //Stores the highest frequency bin that will be displayed as correct 
            List<Bitmap> outListOfPlayedNotes = new List<Bitmap>();

            Bitmap currentLine = new Bitmap(p_listOfRightNotes[0].Width, p_listOfRightNotes[0].Height);
            Graphics currentLineGraphics = Graphics.FromImage(currentLine);

            Bitmap longGeneratedBitmap;      //Bitmap to store the generated image as it is being made
            Graphics generatedBitmapGraphics;  //Graphics that will handle all the drawing

            //try {

            //Generate a bitmap that will be the hgiehgt required and long enough that it can hold the entire graph of what was played, which will then be chopped up into each induvidual lines before being saved to the global thing
            longGeneratedBitmap = new Bitmap((int)(audioDetected.Count * pixelsPerSecond / 100), p_listOfRightNotes[0].Height);

            int binHeight = longGeneratedBitmap.Height / (largestBinNum - lowestBinNum);   //Stores the height in px of each bin



            //This bit populates the l o n g bitmap with the notes that the user played
            using(Pen linePen = new Pen(Brushes.Red,binHeight))        //The pen to use will be red and the height of one bin line on the example
            using (generatedBitmapGraphics = Graphics.FromImage(longGeneratedBitmap)) {
                for (int i = 0; i < audioDetected.Count && i < audioBins.Count * 10; i++) {     //i in this for loop represents 10ms so the scale factor 100 is used a lot to make everything baised on seconds
                    //If there was audio being played at that time, then draw a line in the right place on the image
                    if (audioDetected[i]) {
                        generatedBitmapGraphics.DrawLine(linePen, i * pixelsPerSecond / 100, longGeneratedBitmap.Height - (audioBins[i / 10] - lowestBinNum) * binHeight, (i + 1) * pixelsPerSecond / 100, longGeneratedBitmap.Height - (audioBins[i / 10] - lowestBinNum) * binHeight);
                    }
                }
            }


            //This bit chops up the l o n g bitmap into stave lengths
            for (int currentXPos = 0; currentXPos <= longGeneratedBitmap.Width; currentXPos += currentLine.Width) {
                currentLine = new Bitmap(currentLine.Width, currentLine.Height);            //Clear the bitmap after each use
                currentLineGraphics = Graphics.FromImage(currentLine);
                currentLineGraphics.DrawImage(longGeneratedBitmap, -currentXPos, 0);        //get the section of the l o n g image that will be the current line
                outListOfPlayedNotes.Add(new Bitmap(currentLine));                              //Add the current line to the array
                //outListOfPlayedNotes.Add(longGeneratedBitmap.Clone(new Rectangle(currentXPos, 0, p_listOfRightNotes[0].Width, p_listOfRightNotes[0].Height), System.Drawing.Imaging.PixelFormat.DontCare));
            }

            /* } catch (Exception) {

                 errorString = "Try catch error";
                 return false;
             }*/

            p_listOfPlayedBitmaps = outListOfPlayedNotes;
            
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
