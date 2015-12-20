using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;

using Android.Widget;
using GHSE_Online.Fragments;
using System;
using System.Collections.Generic;
using System.Net;

namespace GHSE_Online.Activities
{
    [Activity(Label = "Home", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : BaseAcivity
    {

        public static long timestamp = 0;
        public static List<Post> newsList = new List<Post>();

        public static bool summertime = false;
       public static string fetchedData = "";
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        public static Activity context;
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.layout_main;
            }
        }
        ProgressBar karmapro;
        TextView karma;
        TextView name;
        private AlertDialog.Builder dialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            if (Userinfo.UserHash != "")
            {
                dialog = new AlertDialog.Builder(Application.Context);
                name = (TextView)FindViewById(Resource.Id.textViewName);
                karma = (TextView)FindViewById(Resource.Id.textViewKarma);
                karmapro = (ProgressBar)FindViewById(Resource.Id.progressBarKarma);
                context = this;
                timestamp = Unix_Timestamp.UnixTimeNow();
                drawerLayout = this.FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

                //Set hamburger items menu
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

                //setup navigation view
                navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);

                //handle navigation
                navigationView.NavigationItemSelected += (sender, e) =>
                {
                    e.MenuItem.SetChecked(true);

                    switch (e.MenuItem.ItemId)
                    {
                        case Resource.Id.nav_news:
                            ListItemClicked(0);
                            
                            break;
                        case Resource.Id.nav_uploads:
                            ListItemClicked(1);
                            break;
                        case Resource.Id.nav_inbox:
                            ListItemClicked(2);
                            break;
                        case Resource.Id.nav_logout:
                            Userinfo.logout(this);
                            break;
                    }



                    drawerLayout.CloseDrawers();
                };


                WebClient client = new WebClient();
                string result = "";
                ProgressDialog progress = ProgressDialog.Show(this, "Please wait...", "Fetching data...", true);
                progress.SetCancelable(false);
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
                    switch (result)
                    {
                        case "":

                            Userinfo.logout(this);

                            break;
                        case "wrong hash":                                
                               Userinfo.logout(this);

                            break;
                        default:
                            Userinfo.fetchedData = result;
                            var jsn = Newtonsoft.Json.Linq.JObject.Parse(Userinfo.fetchedData);
                            name.Text = ((string)jsn["fname"] + " " + (string)jsn["lname"]);
                            karma.Text = ((string)jsn["karma"] + "/" + (string)jsn["maxkarma"] + " Karma");
                            karmapro.Max = ((int)jsn["maxkarma"]);
                            karmapro.Progress = ((int)jsn["karma"]);
                            if (Userinfo.writeFile(Userinfo.UserHash + "," + Userinfo.uname))
                            {
                                Console.WriteLine("Loggin saved!");
                            }


                            
                            progress.Dismiss();
                            //if first time you will want to go ahead and click first item.
                            if (savedInstanceState == null)
                            {
                                ListItemClicked(0);

                            }
                            break;
                    }
                   
                };
                client.DownloadStringAsync(new System.Uri(Login.serverAdress + "fetch/data.php?hash=" + Userinfo.UserHash + "&fetch=users" + "&username=" + Userinfo.uname));



               
            }
            else
            {
                Finish();
                Userinfo.uname = "";

            }
        }

      

        



        int oldPosition = -1;
        private void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition)
                return;

            oldPosition = position;

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = news.NewInstance();
                    
                

                    break;
                case 5:

                    break;
                case 2:
                    fragment = Chat.NewInstance();
                    break;
            }

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

