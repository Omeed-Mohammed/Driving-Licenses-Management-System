using Driving_License_Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Driving_License.Global_Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            try
            {
                //this will get the current project directory folder.
                string currentDirectory = Directory.GetCurrentDirectory();


                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + "\\data.txt";

                //incase the username is empty, delete the file
                if (Username == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;

                }

                // concatenate username and password withe separator.
                string dataToSave = Username + "#//#" + Password;

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the data to the file
                    writer.WriteLine(dataToSave);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            //this will get the stored username and password and will return true if found and false if not found.
            try
            {
                //gets the current project's directory
                string currentDirectory = Directory.GetCurrentDirectory();

                // Path for the file that contains the credential.
                string filePath = currentDirectory + "\\data.txt";

                // Check if the file exists before attempting to read it
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // Output each line of data to the console
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            Username = result[0];
                            Password = result[1];
                        }
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }

        //*********************************************************************************************************

        //Ai Code
        //public static bool RememberUsernameAndPassword(string username, string password)
        //{
        //    try
        //    {
        //        // تحديد مسار الملف في الدليل الحالي
        //        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

        //        // حذف الملف إذا كان اسم المستخدم فارغًا
        //        if (string.IsNullOrEmpty(username) && File.Exists(filePath))
        //        {
        //            File.Delete(filePath);
        //            return true;
        //        }

        //        // التحقق من أن اسم المستخدم وكلمة المرور ليسا فارغين قبل الكتابة
        //        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //        {
        //            MessageBox.Show("اسم المستخدم أو كلمة المرور غير صالح.");
        //            return false;
        //        }

        //        // استخدام تشفير أساسي (يفضل تشفير أقوى في بيئات الإنتاج) لتخزين البيانات الحساسة
        //        string dataToSave = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(username + "#//#" + password));

        //        // الكتابة إلى الملف باستخدام 'using' لضمان إغلاقه بشكل آمن
        //        File.WriteAllText(filePath, dataToSave);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"حدث خطأ: {ex.Message}");
        //        return false;
        //    }
        //}


        //public static bool GetStoredCredential(ref string username, ref string password)
        //{
        //    try
        //    {
        //        تحديد مسار الملف باستخدام Path.Combine
        //        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.txt");

        //        تحقق من وجود الملف
        //        if (!File.Exists(filePath))
        //            return false;

        //        قراءة البيانات من الملف
        //        string line = File.ReadAllText(filePath);
        //        string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

        //        تأكد من أن التقسيم أعطى نتيجتين
        //        if (result.Length != 2)
        //            return false;

        //        تعيين اسم المستخدم وكلمة المرور
        //       username = result[0];
        //        password = result[1];

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"حدث خطأ: {ex.Message}");
        //        return false;
        //    }
        //}


    }
}
