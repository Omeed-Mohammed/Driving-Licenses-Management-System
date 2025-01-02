using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_DataAccess
{
    public class clsDetainedLicenseData
    {
        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";
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
                catch (Exception ex) { }
            }
            return dt;
        }

        public static bool GetDetainedLicenseInfoByID(int DetainID,
            ref int LicenseID, ref DateTime DetainDate,
            ref float FineFees, ref int CreatedByUserID,
            ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;
            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", DetainID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            LicenseID = (int)reader["LicenseID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];

                            IsReleased = (bool)reader["IsReleased"];

                            if (reader["ReleaseDate"] == DBNull.Value)

                                ReleaseDate = DateTime.MaxValue;
                            else
                                ReleaseDate = (DateTime)reader["ReleaseDate"];


                            if (reader["ReleasedByUserID"] == DBNull.Value)

                                ReleasedByUserID = -1;
                            else
                                ReleasedByUserID = (int)reader["ReleasedByUserID"];

                            if (reader["ReleaseApplicationID"] == DBNull.Value)

                                ReleaseApplicationID = -1;
                            else
                                ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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
                    //Console.WriteLine("Error: " + ex.Message);
                    isFound = false;
                }
            }
            return isFound;
        }


        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
         ref int DetainID, ref DateTime DetainDate,
         ref float FineFees, ref int CreatedByUserID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;
            string query = "SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID order by DetainID desc";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            // The record was found
                            isFound = true;

                            DetainID = (int)reader["DetainID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];

                            IsReleased = (bool)reader["IsReleased"];

                            if (reader["ReleaseDate"] == DBNull.Value)

                                ReleaseDate = DateTime.MaxValue;
                            else
                                ReleaseDate = (DateTime)reader["ReleaseDate"];


                            if (reader["ReleasedByUserID"] == DBNull.Value)

                                ReleasedByUserID = -1;
                            else
                                ReleasedByUserID = (int)reader["ReleasedByUserID"];

                            if (reader["ReleaseApplicationID"] == DBNull.Value)

                                ReleaseApplicationID = -1;
                            else
                                ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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
                    //Console.WriteLine("Error: " + ex.Message);
                    isFound = false;
                }
            }

            return isFound;
        }

        //*****************************************************************************************************************************************

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;
            string query = @"INSERT INTO dbo.DetainedLicenses (LicenseID,DetainDate,FineFees,CreatedByUserID,IsReleased)
                            VALUES (@LicenseID,@DetainDate, @FineFees, @CreatedByUserID,0);
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@DetainDate", DetainDate);
                command.Parameters.AddWithValue("@FineFees", FineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        DetainID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);

                }
            }
            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID,int LicenseID, DateTime DetainDate,float FineFees, int CreatedByUserID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID,   
                              WHERE DetainID=@DetainID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@DetainDate", DetainDate);
                command.Parameters.AddWithValue("@FineFees", FineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
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

        //*****************************************************************************************************************************************
        public static bool ReleaseDetainedLicense(int DetainID,int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", DetainID);
                command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
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

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            string query = @"select IsDetained=1 from detainedLicenses where LicenseID=@LicenseID and IsReleased=0;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        IsDetained = Convert.ToBoolean(result);
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);

                }
            }
            return IsDetained;
        }
    }
}
