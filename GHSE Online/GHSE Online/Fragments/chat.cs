
using Android.OS;
using Android.Views;
using Android.App;
using Android.Widget;
using GHSE_Online.Activities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GHSE_Online.Fragments
{
    public class Chat : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
        }
       
        public class ChatMessage{
            public string msg { get; set; }
            public string timestamp { get; set; }
            public string fname { get; set; }
            public string formid { get; set; }
            public string uid { get; set; }
            public string id { get; set; }
        }


        long lastTimestamp = 0;
        long timestamp;
        int i = 0;
        int ID = 0;
        public void fetchChat()
        {

            ListView lw = (ListView)Activity.FindViewById(Resource.Id.msgview);
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (p2, q1) =>
            {
                string result1;
                if (q1.Error != null)
                {
                    result1 = "";
                }
                else
                {
                    result1 = q1.Result;
                }

                string json = result1;
                if (json != "")
                {
                    IList<ChatMessage> roles = new List<ChatMessage>();

                    JsonTextReader reader = new JsonTextReader(new StringReader(json));
                    reader.SupportMultipleContent = true;

                    while (true)
                    {
                        if (!reader.Read())
                            break;

                        JsonSerializer serializer = new JsonSerializer();
                        ChatMessage role = serializer.Deserialize<ChatMessage>(reader);
                       
                        roles.Add(role);
                    }


                    WebClient timestampClient = new WebClient();
                    Unix_Timestamp unix = new Unix_Timestamp();
                    List<MessageChat> messageList = new List<MessageChat>();
                    long newTimestamp = 0;
                    foreach (ChatMessage role in roles)
                    {
                        MessageChat p1 = new MessageChat();
                        p1.msg = role.msg;
                        p1.fname = role.fname;
                        p1.timestamp = Unix_Timestamp.ConvertFromUnixTimestamp(int.Parse(role.timestamp)).ToString();
                        p1.id = role.id;
                        newTimestamp = long.Parse(role.timestamp);
                        messageList.Add(p1);
                       
                        // messageList.Add(p1);



                    }
                    if (newTimestamp != lastTimestamp)
                    {
                        lastTimestamp = newTimestamp;
                        Activity.RunOnUiThread(() =>
                        {
                            lw.Adapter = new Adapter_Chat(Activity, messageList);
                        });
                    }
                    else
                    {

                    }
                }



            };
            client.DownloadStringAsync(new System.Uri(Activities.Userinfo.severURL + "fetch/data.php?hash=" + Activities.Userinfo.UserHash + "&fetch=livechat" + "&timestamp=" + timestamp + "&sorting=asc"));

         

        }
       


        System.Timers.Timer t;
        protected void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            WebClient client = new WebClient();
            fetchChat();
            t.Start();
        }
      
      

       

        AlertDialog.Builder dialog = new AlertDialog.Builder(MainActivity.context);
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            
            timestamp = MainActivity.timestamp;
           
            EditText et = (EditText)Activity.FindViewById(Resource.Id.msg);
            WebClient client = new WebClient();

            t = new System.Timers.Timer();
            t.Interval = 3000;
            t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);

            Button btnsend = (Button)Activity.FindViewById(Resource.Id.send);
            btnsend.Click += delegate
            {
                string msgtosend = et.Text;
                et.Text = "";
                if (msgtosend != "")
                {
                    WebClient sendMSG = new WebClient();
                    sendMSG.DownloadStringCompleted += (p, q) =>
                    {
                        string result;
                        if (q.Error == null && q.Result != "")
                        {
                            result = q.Result;
                        }
                        else
                        {
                            result = "false";
                        }
                        switch (result)
                        {
                            case "wrong hash":
                                dialog.SetMessage("Bitte logge dich erneut ein.");
                                dialog.SetTitle("Error!");
                                dialog.Show();
                                Userinfo.logout(MainActivity.context);
                                break;
                            case "true":
                                fetchChat();
                                break;
                            case "false":
                                dialog.SetMessage("Nachricht konnte nicht gesendet werde.");
                                dialog.SetTitle("Error!");
                                dialog.Show();
                                break;

                        }

                    };
                    sendMSG.DownloadStringAsync(new Uri(Userinfo.severURL+"fetch/nojson.php?msg="+msgtosend+"&hash=" + Userinfo.UserHash));
                }
                
           };
            fetchChat();
            t.Start();

        }

        public static Chat NewInstance()
        {
            var frag1 = new Chat { Arguments = new Bundle() };
            return frag1;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.layout_chat, null);
        }
    }
}