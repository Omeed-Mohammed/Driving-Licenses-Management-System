using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Driving_License_DataAccess
{
    public class clsPersonData
    {
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM PeopleInfo";
            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
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
                    Console.WriteLine("Error : Not People Found " + ex.Message);
                    return null;
                }
                return dt;
            }
        }

        //*********************************************************************************************************
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref short Gender, ref string Address, ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            FirstName = reader["FirstName"] as string ?? "";
                            SecondName = reader["SecondName"] as string ?? "";
                            ThirdName = reader["ThirdName"] as string ?? "";
                            LastName = reader["LastName"] as string ?? "";
                            NationalNo = reader["NationalNo"] as string ?? "";
                            DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.MinValue;
                            Gender = reader["Gender"] != DBNull.Value ? (short)Convert.ToInt16(reader["Gender"]) : (short)0;
                            Address = reader["Address"] as string ?? "";
                            Phone = reader["Phone"] as string ?? "";
                            Email = reader["Email"] as string ?? "";
                            NationalityCountryID = reader["NationalityCountryID"] != DBNull.Value ? (int)reader["NationalityCountryID"] : 0;
                            ImagePath = reader["ImagePath"] as string ?? "";
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
                    Console.WriteLine("Error: No Person Found " + ex.Message);
                    isFound = false;
                }
                return isFound;
            }

                   
            
   
        }

        //*********************************************************************************************************

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gender, ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {               
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            FirstName = reader["FirstName"] as string ?? "";
                            SecondName = reader["SecondName"] as string ?? "";
                            ThirdName = reader["ThirdName"] as string ?? "";
                            LastName = reader["LastName"] as string ?? "";
                            DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.MinValue;
                            Gender = reader["Gender"] != DBNull.Value ? (short)Convert.ToInt16(reader["Gender"]) : (short)0;
                            Address = reader["Address"] as string ?? "";
                            Phone = reader["Phone"] as string ?? "";
                            Email = reader["Email"] as string ?? "";
                            NationalityCountryID = reader["NationalityCountryID"] != DBNull.Value ? (int)reader["NationalityCountryID"] : 0;
                            ImagePath = reader["ImagePath"] as string ?? "";
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
                    Console.WriteLine("Error: No Person Found " + ex.Message);
                    isFound = false;
                }
                return isFound;
            }
        }

        //*********************************************************************************************************
        public static int AddNewPerson(string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;
            string query = @"
        INSERT INTO People (FirstName, SecondName, ThirdName, LastName, NationalNo,
                            DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath)
        VALUES (@FirstName, @SecondName, @ThirdName, @LastName, @NationalNo,
                @DateOfBirth, @Gender, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);
        SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", FirstName);
                command.Parameters.AddWithValue("@SecondName", SecondName);
                command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
                command.Parameters.AddWithValue("@LastName", LastName);
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                command.Parameters.AddWithValue("@Gender", Gender);
                command.Parameters.AddWithValue("@Address", Address);
                command.Parameters.AddWithValue("@Phone", Phone);
                command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
                command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        PersonID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                }
            }
            return PersonID;
        }

        //*********************************************************************************************************

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gender=@Gender,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                //command.Parameters.AddWithValue("@FirstName", FirstName);
                //command.Parameters.AddWithValue("@SecondName", SecondName);
                //command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
                //command.Parameters.AddWithValue("@LastName", LastName);
                //command.Parameters.AddWithValue("@NationalNo", NationalNo);
                //command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                //command.Parameters.AddWithValue("@Gender", Gender);
                //command.Parameters.AddWithValue("@Address", Address);
                //command.Parameters.AddWithValue("@Phone", Phone);
                //command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
                //command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                //command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);

                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                command.Parameters.Add("@SecondName", SqlDbType.NVarChar).Value = SecondName;
                command.Parameters.Add("@ThirdName", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                command.Parameters.Add("@NationalNo", SqlDbType.NVarChar).Value = NationalNo;
                command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime).Value = DateOfBirth;
                command.Parameters.Add("@Gender", SqlDbType.SmallInt).Value = Gender;
                command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = Phone;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email;
                command.Parameters.Add("@NationalityCountryID", SqlDbType.Int).Value = NationalityCountryID;
                command.Parameters.Add("@ImagePath", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath;
                command.Parameters.Add("@PersonID", SqlDbType.Int).Value = PersonID;


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

        //*********************************************************************************************************
        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;
            string query = @"Delete People where PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    connection.Open();

                    rowsAffected = command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }
            }
            return (rowsAffected > 0);
        }

        //*********************************************************************************************************
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;
            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;
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

        //*********************************************************************************************************
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;
            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NationalNo", NationalNo);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;
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

    }
}
