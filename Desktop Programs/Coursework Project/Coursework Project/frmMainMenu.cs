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



        #endregion

    }
}