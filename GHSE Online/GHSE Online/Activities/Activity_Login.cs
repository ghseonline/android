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
    [Activity(Label = "GHSE-Online", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/Icon_GHSE")]
    public  class About : ActionBarActivity
    {
      

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.layout_about);
            var toolbar1 = FindViewById<Toolbar>(Resource.Id.toolbar_activity);

            //Toolbar will now take on default actionbar characteristics
            SetSupportActionBar(toolbar1);
            SupportActionBar.Title = "About";
           
        }
    }
}