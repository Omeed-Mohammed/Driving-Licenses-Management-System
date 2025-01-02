using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_DataAccess
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;
            string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {               
                command.Parameters.AddWithValue("@CountryID", ID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                        CountryName = (string)reader["CountryName"];
                    }
                    else
                    {
                        // The record was not found
                        isFound = false;
                    }

                    reader.Close();

                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("Error Cannot Find Country" + ex.Message);
                    isFound = false;
                    CountryName = null;
                }
                

                return isFound;
            }
            
        }
        //**********************************************************************************************
        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool isFound = false;
            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CountryName", CountryName);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                       // The record was found
                        isFound = true;
                        ID = (int)reader["CountryID"];
                    }
                    else
                    {
                        // The record was not found
                        isFound = false;
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("Error Cannot Find Country" + ex.Message);
                    isFound = false;
                    CountryName = null;
                }
                
                return isFound;
            }

                

            
        }
        //**********************************************************************************************
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Countries order by CountryName";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }               
                return dt;
            }
        }
    }
}
