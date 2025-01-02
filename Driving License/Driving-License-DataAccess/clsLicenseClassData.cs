using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_DataAccess
{
    public class clsLicenseClassData
    {
        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM LicenseClasses";
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

        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription
            , ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(@"LicenseClassID", LicenseClassID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            ClassName = (string)reader["ClassName"];
                            ClassDescription = (string)reader["ClassDescription"];
                            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
                        }
                        else
                        {
                            isFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }
            return isFound;

        }

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
            ref string ClassDescription, ref byte MinimumAllowedAge,
           ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(@"ClassName", ClassName);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            LicenseClassID = (int)reader["LicenseClassID"];
                            ClassDescription = (string)reader["ClassDescription"];
                            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
                        }
                        else
                        {
                            isFound = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }
            return isFound;
        }



        //***************************************************************************************************************************

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;
            string query = @"Insert Into LicenseClasses 
           (
            ClassName,ClassDescription,MinimumAllowedAge, 
            DefaultValidityLength,ClassFees)
                            Values ( 
            @ClassName,@ClassDescription,@MinimumAllowedAge, 
            @DefaultValidityLength,@ClassFees)
                            where LicenseClassID = @LicenseClassID;
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClassName", ClassName);
                command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                command.Parameters.AddWithValue("@ClassFees", ClassFees);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        LicenseClassID = insertedID;
                    }
                }
                catch (Exception ex) { }
            }
            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int rowsAffected = 0;
            string query = @"Update  LicenseClasses  
                            set ClassName = @ClassName,
                                ClassDescription = @ClassDescription,
                                MinimumAllowedAge = @MinimumAllowedAge,
                                DefaultValidityLength = @DefaultValidityLength,
                                ClassFees = @ClassFees
                                where LicenseClassID = @LicenseClassID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                command.Parameters.AddWithValue("@ClassName", ClassName);
                command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                command.Parameters.AddWithValue("@ClassFees", ClassFees);

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
