using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_DataAccess
{
    public class clsTestTypeData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TestTypes";
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

        public static bool GetTestTypeInfoByID(int TestTypeID,
            ref string TestTypeTitle, ref string TestDescription, ref float TestFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            TestTypeTitle = (string)reader["TestTypeTitle"];
                            TestDescription = (string)reader["TestTypeDescription"];
                            TestFees = Convert.ToSingle(reader["TestTypeFees"]);
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

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;
            string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeDescription,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@ApplicationFees)
                            where TestTypeID = @TestTypeID;
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeTitle", Title);
                command.Parameters.AddWithValue("@TestTypeDescription", Description);
                command.Parameters.AddWithValue("@ApplicationFees", Fees);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestTypeID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                }
            }
            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {
            int rowsAffected = 0;
            string query = @"Update  TestTypes  
                            set TestTypeTitle = @TestTypeTitle,
                                TestTypeDescription=@TestTypeDescription,
                                TestTypeFees = @TestTypeFees
                                where TestTypeID = @TestTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@TestTypeTitle", Title);
                command.Parameters.AddWithValue("@TestTypeDescription", Description);
                command.Parameters.AddWithValue("@TestTypeFees", Fees);

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
