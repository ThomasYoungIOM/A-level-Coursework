using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;



namespace Coursework_Project {
    /// <summary>
    /// A class that provides a simple to use interface for all of the database functions that are required for the program
    /// </summary>
    public class databaseInterface {
        private string p_connectionString;      //Stores the connection string for the database connection
        
        /// <summary>
        /// Initilises the class with the specifided connection string 
        /// </summary>
        /// <param name="connectionStringIn">The connection string that will be used to create the database connections</param>
        public databaseInterface(string connectionStringIn) {
            p_connectionString = connectionStringIn;
        }

        /// <summary>
        /// Executes the specified nonquery command on the database
        /// </summary>
        /// <param name="inputCommand">The non-query to execute</param>
        public void executeNonQuery(SqlCommand inputCommand) {

            //Using to make sure everything is disposed of after it's use so it does not use resourses for any longer than necerssary
            using (SqlConnection dbConnection = new SqlConnection(p_connectionString))
            using (inputCommand) {
                dbConnection.Open();                        //Open the database connection
                inputCommand.Connection = dbConnection;     //Tell the command what database connection to use
                inputCommand.ExecuteNonQuery();             //Executes the nonquery
            }
        }


        /// <summary>
        /// Executes the query command and returns the 
        /// </summary>
        /// <param name="inputCommand"></param>
        /// <returns></returns>
        public DataTable executeQuery(SqlCommand inputCommand) {

            //Using to make sure everything is disposed of after it's use so it does not use resourses for any longer than necerssary
            using (SqlConnection dbConnection = new SqlConnection(p_connectionString))      //The connection to the database that will be used. Specified using the connection string passed when the class is initilised
            using (inputCommand)                                                            //The command that will be run
            using (SqlDataAdapter adapter = new SqlDataAdapter(inputCommand))               //The adapter that will actually get the data from the database and turn it into the table. The adapter is given the command to execute here.
            using (DataTable outputDataTable = new DataTable()) {                            //The datatable that will store and return the output from the query

                dbConnection.Open();                    //Opens the database connection
                inputCommand.Connection = dbConnection; //Tells the command what connection to use
                adapter.Fill(outputDataTable);          //Fill the datatable with the output from the query
                return outputDataTable;                 //Returns the output
            }
        }

        /// <summary>
        /// Executes a Scalar query on the database and returns the output object
        /// </summary>
        /// <param name="inputCommand">The scalar command to execute</param>
        /// <returns>The object result from the scalar </returns>
        public int executeIntScalarQuery(SqlCommand inputCommand) {

            //Using to make sure everything is disposed of after it's use so it does not use resourses for any longer than necerssary
            using (SqlConnection dbConnection = new SqlConnection(p_connectionString))  //The connection to the database that will be used. Specified using the connection string passed when the class is initilised
            using (inputCommand) {                                                      //The command that will be run

                dbConnection.Open();                                            //Opens the database connection
                inputCommand.Connection = dbConnection;                         //Tells the command what connection to use
                return int.Parse(inputCommand.ExecuteScalar().ToString());      //Returns the output object, converted to an int
            }
        }
    }
}
