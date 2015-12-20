
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using GHSE_Online.Activities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GHSE_Online.Fragments
{
    public class news : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
        }
        AlertDialog.Builder dialog = new AlertDialog.Builder(MainActivity.context);
       
        public class Role{
            public string msg { get; set; }
            public string timestamp { get; set; }
        }



        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            ProgressBar pb = (ProgressBar)Activity.FindViewById(Resource.Id.newsprogress);
            ListView et = (ListView)Activity.FindViewById(Resource.Id.NewsView);
            TextView tv = (TextView)Activity.FindViewById(Resource.Id.textViewNews1);

            var jsn = Newtonsoft.Json.Linq.JObject.Parse(Activities.Userinfo.fetchedData);

            pb.Visibility = ViewStates.Visible;

            tv.Text = ("Hallo " + (string)jsn["fname"] + "!");
            WebClient client = new WebClient();
            if (MainActivity.newsList.Count <= 0)
            {

                client.DownloadStringCompleted += (p, q) =>
            {
                string result;
                if (q.Error != null)
                {
                    result = "";
                }
                else
                {
                    result = q.Result;
                }

                string json = result;
                switch (result)
                {
                    case "":
                        dialog.SetMessage("Bitte überprüfe deine Internetverbindung!");
                        dialog.SetTitle("Error!");
                        dialog.Show();

                        pb.Visibility = ViewStates.Gone;
                        break;
                    case "wrong hash":
                        dialog.SetMessage("Bitte logge dich erneut ein!");
                        dialog.SetTitle("Error!");
                        Userinfo.logout(MainActivity.context);

                        break;
                    default:

                        IList<Role> roles = new List<Role>();
                        JsonTextReader reader = new JsonTextReader(new StringReader(json));
                        reader.SupportMultipleContent = true;
                        while (true)
                        {
                            if (!reader.Read())
                                break;
                            JsonSerializer serializer = new JsonSerializer();
                            Role role = serializer.Deserialize<Role>(reader);
                            roles.Add(role);
                        }


                        foreach (Role role in roles)
                        {
                            Post p1 = new Post();
                            if (role.msg != "" && p1.time != "0" && p1.time != "")
                            {
                                p1.msg = role.msg;
                                p1.time = Unix_Timestamp.ConvertFromUnixTimestamp(int.Parse(role.timestamp)).ToString();
                                MainActivity.newsList.Add(p1);
                            }
                        }


                        et.Adapter = new NewsAdapter(Activity, MainActivity.newsList);

                        pb.Visibility = ViewStates.Gone;
                        break;

                }
            };
            client.DownloadStringAsync(new System.Uri(Activities.Userinfo.severURL + "fetch/data.php?hash=" + Activities.Userinfo.UserHash + "&fetch=news"));
            }
            else
            {
                et.Adapter = new NewsAdapter(Activity, MainActivity.newsList);

                pb.Visibility = ViewStates.Gone;
            }


        }
        

        public static news NewInstance()
        {
            var frag1 = new news { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.layout_news, null);
        }
    }
}