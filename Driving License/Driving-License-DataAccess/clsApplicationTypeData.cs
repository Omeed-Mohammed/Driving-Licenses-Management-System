using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_DataAccess
{
    public class clsApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM ApplicationTypes";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }

                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error : Not People Found " + ex.Message);
                    return null;
                }
                return dt;
            }
        }

        //********************************************************************************
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
            ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                            ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: No Person Found " + ex.Message);
                    isFound = false;
                }
                return isFound;
            }
        }

        //********************************************************************************
        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;
            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)    
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                command.Parameters.AddWithValue("@ApplicationFees", Fees);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ApplicationTypeID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                }
            }
            return ApplicationTypeID;
        }

        //********************************************************************************
        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {
            int rowsAffected = 0;
            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Fees", Fees);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
            return (rowsAffected > 0);
        }



    }
}
