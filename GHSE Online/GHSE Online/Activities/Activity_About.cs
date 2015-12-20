using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Net;

	
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace GHSE_Online.Activities
{
    [Activity(Label = "GHSE-Online", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon_GHSE")]
    public  class Login : ActionBarActivity
    {
      

        public static string serverAdress = "http://ghse-online.de/api/";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layout_login);
            var toolbar1 = FindViewById<Toolbar>(Resource.Id.toolbar_activity);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar1);
            SupportActionBar.Title = "Login";
            if (Userinfo.readLogin())
            {
                StartActivity(typeof(MainActivity));

                Console.WriteLine("Found login!");
                Finish();
            }
            else
            {
                Console.WriteLine("No login found!");
            }


            Button btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
            btnLogin.Click += delegate
            {
                ProgressDialog progress = ProgressDialog.Show(this, "Loading...", "Please Wait...", true);
                EditText username = (EditText)FindViewById(Resource.Id.username);
                EditText password = (EditText)FindViewById(Resource.Id.password);

                WebClient client = new WebClient();
                string result = "";


                client.DownloadStringCompleted += (p, q) =>
                {
                    if (q.Error == null)
                    {
                        result = q.Result;
                    }
                    else
                    {
                        result = "";
                    }
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    switch (result)
                    {
                        case "false":

                            dialog.SetMessage("Bitte überprüfe deine Eingaben.");
                            dialog.SetTitle("Error!");
                            dialog.Show();
                            break;
                        case "":

                            dialog.SetMessage("Bitte überprüfe deine Internetverbindung!");
                            dialog.SetTitle("Error!");
                            dialog.Show();
                            break;
                        default:
                            Userinfo.UserHash = result;
                            Userinfo.uname = username.Text;
                            StartActivity(typeof(MainActivity));
                            Finish();
                            break;
                    }
                    progress.Dismiss();

                };
                client.DownloadStringAsync(new System.Uri(serverAdress + "checklogin.php?username=" + username.Text + "&password=" + generateHash.SHA512StringHash(password.Text)));

               
            };
        }
    }
}