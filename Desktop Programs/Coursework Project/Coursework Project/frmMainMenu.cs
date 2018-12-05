using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Coursework_Project {

    public partial class frmMainMenu : Form {
        public frmMainMenu() {
            InitializeComponent();
        }


        const string dbConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\Thomas\source\repos\A-level Coursework\Desktop Programs\Coursework Project\CourseworkDatabase.mdf';Integrated Security=True;Connect Timeout=30";

        static midiFile loadedFile;        //The object that will store the loaded MIDI file


        #region Form Event Triggered Subs

        /// <summary>
        /// Run when form loads.
        /// Checks all the files are there
        /// Gets a list of instruments and adds it to the comboBox
        /// Populates dgvExercises with data from the database
        /// </summary>
        private void frmMainMenu_Load(object sender, EventArgs e) {
            #region Varibles
            databaseInterface databaseInterface = new databaseInterface(dbConnectionString);        //The database class that will allow me to interact with the database easily
            SqlCommand currentCommand;
            #endregion

            //Check all the files in the database to see if they are there
            checkDatabaseFiles(databaseInterface);

            //Get a list of all of the instruments of the availble exercises and adds them to the Combo box view. 
            currentCommand = new SqlCommand("SELECT DISTINCT Instrument FROM Exercises WHERE FileExists = 1 INTERSECT SELECT DISTINCT Instrument FROM Notes");      //This command gets a list of all of the instruments that can be found in the Exercises table AND in the notes table. This ensures that there will be no exercises selected for which there aren't any defininitions for that instrument
            cmbInstuments.Items.AddRange(databaseInterface.executeQuery(currentCommand).AsEnumerable().Select(row => row[0].ToString()).ToArray());                 //The "AsEnumerable" converts the datatable into an enumerable that can then have all of the elements selected from it and converted to an array

            //Query database to select all available songs and display them in the datagrid view
            currentCommand = new SqlCommand("SELECT * FROM Exercises WHERE FileExists = 1 AND Exercises.Instrument IN(SELECT Instrument FROM Notes)");      //This command gets all of the exercises where the file can be found and the instrument used in the exercises also has note definitions for it
            dgvExercises.DataSource = databaseInterface.executeQuery(currentCommand);                                                                       //Assign the result from the sql query to dgvExercises
        }

        /// <summary>
        /// Run when the selected instrument in the comboBox is changed
        /// Updates the dgvExercises with exercises that are for the instruemnt that the user selected
        /// </summary>
        private void cmbInstuments_SelectedIndexChanged(object sender, EventArgs e) {
            databaseInterface databaseInterface = new databaseInterface(dbConnectionString);                            //The database class that will allow me to interact with the database
            SqlCommand sqlCommandToRun = new SqlCommand();                                      //The Sql Statement that will be used to populate dgvExercises

            //If the "All" option is selected, then set the statement to one that will select all of the exercises, else, just select the ones with the instrument there
            if (cmbInstuments.SelectedItem.ToString() == "All") {
                sqlCommandToRun.CommandText = "SELECT * FROM Exercises WHERE FileExists = 1 AND Exercises.Instrument IN(SELECT Instrument FROM Notes)";         //Sql Command that will select all the exercises that use instruments for which there are not definitions
            } else {
                sqlCommandToRun.CommandText = "SELECT * FROM Exercises WHERE FileExists = 1 AND Exercises.Instrument IN(SELECT Instrument FROM Notes) AND Instrument = @instrument";        //Sql Commmand that will select all the exercises that use the right instrument
                sqlCommandToRun.Parameters.AddWithValue("@instrument", cmbInstuments.SelectedItem.ToString());      //If the selected item is "All", then set the parameter to *, else set it to the inputted string
            }

            dgvExercises.DataSource = databaseInterface.executeQuery(sqlCommandToRun);      //Assign the results of the SQL query to dgvExercises

        }

        /// <summary>
        /// Run when the the user wants to look at the sheet music
        /// Gets the filepath of the exercise that is currently selected in dgvExercises
        /// Instaniats frmSheetMusic and passes the file path to the selected MIDI file across
        /// </summary>
        private void btnLookAtMusic_Click(object sender, EventArgs e) {
            //Make sure the user has selected an exercise
            if (dgvExercises.SelectedRows != null) {
                //Run the form
            } else {
                MessageBox.Show("Please select an exercise to display first");
            }
        }


        private void btnLoadMidi_Click(object sender, EventArgs e) {
            string errorString;         //Stores the error message incase the file cannot be parsed
            string midiFilePath;        //Stores the file path for the exerise
            Stream midiStream;          //Stores the stream of data to be interprated
            bool parseSucessful;        //Stores whether the midi parse was sucessful or not

            midiFilePath = (string)dgvExercises.SelectedRows[0].Cells["filePath"].Value;  //Get midi path from dgv

            try {
                midiStream = File.Open(midiFilePath,FileMode.Open);    //Open file into a stream

                parseSucessful = parseMidiFile(midiStream, out loadedFile, out errorString);         //Parse the MIDI file and set it to the loaded file

                //If there was an error, tell the user
                if (!parseSucessful) {
                    MessageBox.Show("The file could not be Loaded. Please try another file.","MIDI Parse Error");
                }

            } catch (Exception ex) {
                MessageBox.Show($"Sorry, that file could not be opened. Please try again or select a different Exercise. /n {ex}");
            }
        }          





            /// <summary>
            /// Closes the program
            /// </summary>
            private void btnQuit_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        #endregion

        #region Called Subs

        /// <summary>
        /// Checks the database to see if the files can be found. If a file cannot be found, it's "FileExists" row will be set to 0, else it will be set to 1 to show it does exist.
        /// </summary>
        /// <param name="databaseToCheck">The database interface that will be searched</param>
        private void checkDatabaseFiles(databaseInterface databaseToCheck) {
            SqlCommand currentCommand;
            List<string> filesNotFound = new List<string>();        //Stores the list of file paths that do not have files at the other end
            string currentFilePath;                                 //Stores the file path that is currently being checked to see if it exsists
            string generatedSqlString;                              //Stores the generated SQL statement that will be used to update the database with which files are there or not

            //Checks each file in the Database to see if it can be found. If it can't be, then it's filepath is output to a list
            using (currentCommand = new SqlCommand("SELECT FilePath FROM Exercises")) {
                //Loop through all of the paths returned from the database, converts them string, checks they exist. If they do, the ids are saved to one list, else, the ids are saved to another list
                foreach (DataRow currentRow in databaseToCheck.executeQuery(currentCommand).Rows) {

                    currentFilePath = Convert.ToString(currentRow.ItemArray[0]);    //Get the file path from the row

                    //If the file doesn't exsist, then save the filePath to the fileNotFound list
                    if (!File.Exists(currentFilePath))
                        filesNotFound.Add(currentFilePath);
                }
            }

            //Update the datbase so the files are shown not to exist. Else, update athe database so all files are shown to exist
            using (currentCommand = new SqlCommand()) {
                /* In order to update the database to store whether the file can be found or not, an UPDATE statement is used with parametisation.
                 *  
                 *  The parametised statement should look like this for example:
                 *  UPDATE Exercises SET FileExists = CASE 
                 *                                        WHEN FilePath IN( @0, @1, @2, @3, @4) THEN 0
                 *                                        ELSE 1
                 *                                    END;
                 * 
                 * In this statement, the parametised values will be the file paths of files that could not be found
                 */

                generatedSqlString = "UPDATE Exercises SET FileExists = CASE WHEN FilePath IN(NULL";      //The first part of the SQL Statement that will update the database depedning if the files are availible (A default null value is added, so if all files are found, the database is still updated)

                //Loop through the list of files that have not been found
                for (int i = 0; i < filesNotFound.Count; i++) {
                    generatedSqlString += $" @{i},";                                         //Add a paramenter to the end of the statement string. eg: " @5,"
                    currentCommand.Parameters.AddWithValue($"@{i}", filesNotFound[i]);      //Add the parameter and the value that the parameter will assume to the command object which will be passed to the database interface
                }

                generatedSqlString = generatedSqlString.TrimEnd(new char[]{','});                    //Remove the last comma from the string, since it will be the unecerssary comma from the for loop
                generatedSqlString += ") THEN 0 ELSE 1 END;";       //Finish off the SQL Statement

                currentCommand.CommandText = generatedSqlString;    //Assign the generated string to the command object to get executed

                databaseToCheck.executeNonQuery(currentCommand);    //Execute the command
            }
        }


        /// <summary>
        /// Parses the MIDI file into the midiFile class
        /// </summary>
        /// <param name="midiStream">The Stream that should be parsed</param>
        /// <param name="outputMidiFile">The MIDI File class that will be outputted filled with data</param>
        /// <param name="errorString">In case the file could not be parsed, then this will contain the error</param>
        /// <returns>Returns whether the parse was sucsessful</returns>
        private bool parseMidiFile(Stream midiStream, out midiFile outputMidiFile, out string errorString) {
            #region Variables
            byte currentbyte;   //Stores the current byte being processed
            byte statusCarry;   //Stores the last status byte for use by status carry
            bool end = false;   //Has the end of the file been reached?
            uint chunkType;
            uint currentChunkLength;    //Stores the length of the current chunk
            outputMidiFile = null;        //Stores the parsed midiFile
            errorString = null;             //Stores any error messages that are generated

            List<notes> listOfNotes;        //List to store the parsed notes

            ushort format;      //Stores the Midi file's format. (The program can only deal with format 0 MIDI files)
            ushort numOfTracks; //Store the number of track chuhnks to be found in the file. Should be 1 for Format 0 files
            ushort division;    //Store the pace of the MIDI file
            #endregion

            #region Parse Head Chunk

            #region Get data from stream

            //Chunk Type (4 bytes) - Will be either "MThd" for the head chunk or "MTrk" for track chunk in ASCII.
            chunkType = (uint)((midiStream.ReadByte()<<24) | (midiStream.ReadByte() << 16) | (midiStream.ReadByte() << 8) | (midiStream.ReadByte()));       //Gets the chunk type
            
            //Length (4 bytes) - In Head chunk, should always be 6
            currentChunkLength = (uint)((midiStream.ReadByte() << 24) | (midiStream.ReadByte() << 16) | (midiStream.ReadByte() << 8) | (midiStream.ReadByte()));    //Convert the 4 bytes into 1 integer. First byte times by 16^6 plus the second byte times 16^4 and so on

            //Format (2 byte) - Should be either 0, 1 or 2. The program can only deal with format 0 MIDI Files
            format = (ushort)((midiStream.ReadByte() << 8) | (midiStream.ReadByte()));

            //Number of Tracks (2 bytes) - Should be 1 for format 0 MIDI files
            numOfTracks = (ushort)((midiStream.ReadByte() << 8) | (midiStream.ReadByte()));

            //Division (2 bytes) - Stores the speed and the tempo of the MIDI File. Can either store the Number of delta time units in a crochet, or the number of frames per second and the number of ticks per frame. Outlined below.
            division = (ushort)((midiStream.ReadByte() << 8) | (midiStream.ReadByte()));

            /* Bit:     | 15 |    14 - 8    |   7 - 0   |
             *          |  0 |     Ticks per crochet    |
             *          |  1 |-frames/second|ticks/frame|
             */
            #endregion


            #region Validate data

            //Make sure all aspects of the head chunks are valid, if they are not, then output an error and return that the parse failed
            if (chunkType != 0x4d546864) {
                errorString = "Head Chunk title not found";
                return false;
            } else if (currentChunkLength != 6) {
                errorString = "Head Chunk invalid length";
                return false;
            } else if (format != 0){
                errorString = "Format not valid";
                return false;
            } else if (numOfTracks != 1) {
                errorString = "Invalid number of Track Chunks";
                return false;
            }

            #endregion


            #endregion

            #region Parse Track Chunk

            //Chunk Type (4 bytes) - Will be either "MThd" for the head chunk or "MTrk" for track chunk in ASCII.
            chunkType = (uint)((midiStream.ReadByte() << 24) | (midiStream.ReadByte() << 16) | (midiStream.ReadByte() << 8) | (midiStream.ReadByte()));       //Gets the chunk type

            //Length (4 bytes) - In Head chunk, should always be 6
            currentChunkLength = (uint)((midiStream.ReadByte() << 24) | (midiStream.ReadByte() << 16) | (midiStream.ReadByte() << 8) | (midiStream.ReadByte()));    //Convert the 4 bytes into 1 integer. First byte times by 16^6 plus the second byte times 16^4 and so on


            //If the chunk type is not the track chunk, then display the error and exit the function
            if (chunkType != 0x4d54726b) { 
                errorString = "Could not find Track Chunk";
                return false;
            }

            //Read MIDI Track Data. If the track data could not be read properly, end the function
            if(!readTrackData(midiStream, currentChunkLength, out listOfNotes, out errorString))
                return false;
            

            #endregion

            return true;
        }

        private bool readTrackData(Stream midiStream, uint currentChunkLength, out List<notes> listOfNotes, out string errorString) {
            byte currentByte;       //Store the byte currently being processed
            byte statusByte;        //Stores the status byte that 
            byte firstDatabyte;     //Stores the first databyte of the current command
            byte statusCarry = 0;   //Store the last status byte for status carry to be used
            bool end = false;       //Stores whether the end of the chunk has been reached
            int currentDeltaTime = 0;   //Stores the current delta time that will be stored
            int currentNoteLength;
            byte currentNote;       //Stores the current note that is being played

            

            listOfNotes = null;     //Makes sure somthing is assigned to the list of notes
            errorString = "";       //Makes sure somthing is assigned to the error string


            //Loop util the end of the track
            do {

                //Read the deltaTime value
                currentDeltaTime = (currentDeltaTime << 8) + GetVaribleLength(midiStream);

                currentByte = (byte)midiStream.ReadByte();

                //Interprate the status byte.

                //If the current byte is not a status byte, then assign the status carry to the command byte and the current byte to the first databyte
                if (currentByte < 0x80) {
                    firstDatabyte = currentByte;
                    statusByte = statusCarry;

                    //Else assign the current byte to the status byte and read a new byte that will be the first data byte
                } else {
                    statusByte = currentByte;
                    firstDatabyte = (byte)midiStream.ReadByte();
                }




                //If it's a note on, then store the note that was turned on, start counting the delta time, and start counting the time for the next note.
                switch (statusByte & 0xF0) {

                    case 0x80:  //Note off (2 data bytes)
                                //create the note with the specified length
                        listOfNotes.Add(new notes(currentDeltaTime, statusByte, );
                        break;


                    case 0x90:  //Note on (2 data bytes)
                                //save the previous note
                                //Create the new note and start the counting again
                        break;

                    case 0xa0:  //"Polly phonic after touch" (2 data bytes)
                                
                        break;

                    case 0xb0:  //Chan mode control thingys.    (2data bytes)

                        midiStream.ReadByte();
                        break;

                    case 0xc0:  //Chan Program Change           (1 data byte)

                        break;

                    case 0xd0:  //Chanel after touch    (1 data byte)

                        break;

                    case 0xe0:  //Pitch bend (2 data bytes)

                        break;

                    case 0xf0:  //Meta event (Varing databytes)

                        break;
                    default:
                        break;
                }

                //If the current byte is not a meta command, then set the status carry to the current byte
                if (currentByte < 0xf0)
                    statusCarry = currentByte;




            } while (!end);

            /*currentByte = (byte)midiStream.ReadByte();

            //If the current byte is less than 0x80, then it is a databyte and status carry should be used to determine what note should be played, else, the current byte is the status byte
            if (currentByte < 0x80) {

                //If there is nothing in the status carry, then we've got ourselves an error
                if (statusCarry == 0) {
                    errorString = "Status carry called when empty";
                    return false;
                }

                


            } else {

            }*/







            return true;
        }


        /// <summary>
        /// Sub to interprate the status byte and the subsequenct databytes and act upon them. If the byte was a note on event, 
        /// </summary>
        /// <param name="statusByte">The status byte that is to be looked at</param>
        /// <param name="firstDataByte">The first databyte to look at</param>
        /// <param name="midiStream">The stream that is being read</param>
        /// <param name="errorString">Outs any error messages that may have occured</param>
        /// <returns>Returns whether the execution was sucsessfull</returns>
        private bool InterpretStatusByte(byte statusByte, byte firstDataByte, Stream midiStream, out string errorString) {
            errorString = null;

             switch ((statusByte & 0xF0) >> 4) {        //Use the and bitwise operator to only select the left most nibble, then bitshift right to get left most nibble only
                 case 0x8:
                     //Note off (2 data bytes)
                     break;

                 case 0x9:
                     //Note on   (2data bytes)
                     break;

                 case 0xa:
                     //"Polly phonic after touch" (2 data bytes)
                     break;

                 case 0xb:
                     //Chan mode control thingys.    (2data bytes)
                     midiStream.ReadByte();
                     break;

                 case 0xc:
                     //Chan Program Change           (1 data byte)
                     break;

                 case 0xd:
                     //Chanel after touch    (1 data byte)
                     break;

                 case 0xe:
                     //Pitch bend (2 data bytes)
                     break;

                 case 0xf:
                     //Sytem control bytes, (varying databytes)
                     switch (statusByte & 0x0f) {
                         case 0x0f:
                             //If it's a FF byte, it's a meta event and so it'll follow the format: FF <type> <length> <bytes> 
                             //(Found on page 136 of Referance 1, Complete midi specifation)


                             currentByte = firstDataByte;
                             eventLength = Convert.ToByte(midiStream.ReadByte());

                             //Read the next byte that will include the type of system control byte that's being dealt with
                             switch (currentByte) {
                                 //End of file message
                                 case 0x2f:
                                     end = true;
                                     break;

                                 //Meta-event not included
                                 default:
                                     //If the meta event is not used, then just read to the end of it based on the length of the message
                                     for (int i = 0; i < eventLength; i++) {
                                         midiStream.ReadByte();
                                     }
                                     break;
                             }

                             break;

                         default:
                             break;
                     }
                     break;
             }

             return true;
         }
        

        /// <summary>
        /// Returns the value of the the varible length quantity in the stream
        /// </summary>
        /// <param name="inputStream">Stream to read the varible length quantity from</param>
        /// <returns>The value of the varible length time</returns>
        private int GetVaribleLength(Stream inputStream) {
            int currentValue = 0;       //The int that will store the caluclated value whislt it's being calculated
            byte currentByte = 0;       //The byte that stores the current byte being processed

            do {
                currentByte = (byte)inputStream.ReadByte();                     //Get the next byte from the stream
                currentValue = (currentValue << 7) | (currentByte & 0x7f);      //Bit shift the time byte left 7, then or the current byte after removing bit 8
            } while (currentByte>=0x80);
            
            return currentValue;
        }



        #endregion



    }
}