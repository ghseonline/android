using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System.IO;
using System.Security.Cryptography;

namespace GHSE_Online.Activities
{
    class Userinfo
    {
        
        public static string UserHash = "";
        public static string uname = "";
        public static string pword = "";
        public static string fetchedData = "";
        public static string severURL = "http://ghse-online.de/api/";

       private static string path = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "GHSE-Online");
        private static string filename = System.IO.Path.Combine(path, "login.sav");


        public static void logout(Activity context)
        {
            Userinfo.UserHash = "";
            Userinfo.uname = "";
            if (!Userinfo.deleteLogin())
            {
                AlertDialog.Builder dialog = new AlertDialog.Builder(Application.Context);
                dialog.SetTitle("Error!");
                dialog.SetMessage("Login konnte nicht gelöscht werden.");
                dialog.Show();
            }
            context.RunOnUiThread(() =>
            {
                context.StartActivity(typeof(Login));
            });
            context.Finish();
        }

        public static bool writeFile(string text)
        {
           
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(filename))
            {
                File.Delete(filename);
            }

                using (var streamWriter = new StreamWriter(filename, true))
                {
                    streamWriter.WriteLine(Crypt.Encrypt(text, "HYWLKLFN"));
                }

                return  true;
           
            
        }
        public static bool deleteLogin()
        {
            try
            {
                if (!System.IO.File.Exists(filename))
                {
                    return true;
                }
                else
                {
                    System.IO.File.Delete(filename);

                    return true;
                }
                
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public static bool readLogin()
        {
            try {
                using (var streamReader = new StreamReader(filename))
                {
                    string content = Crypt.Decrypt(streamReader.ReadToEnd(),"HYWLKLFN");
                    string[] loginArray = new string[2];
                    loginArray = content.Split(',');
                    UserHash = loginArray[0];
                    uname = loginArray[1];
                }

                return true;
            }catch (Exception ex)
            {
                return false;

            }
            
                
         
           
                
               
           

        }



        
    }
}