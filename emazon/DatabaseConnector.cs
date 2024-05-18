using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DatabaseConnectionExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connection string to your local SQL Server database
            string connectionString = @"Data Source=(local);Initial Catalog=Emazon;Integrated Security=True";

            // Create a SqlConnection object
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Connection is successful
                    Debug.WriteLine("Connected to the database.");

                    // Perform database operations here

                    // Don't forget to close the connection when done
                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Connection failed, display the error message
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
