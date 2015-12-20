using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GHSE_Online.Activities
{
    public class MessageChat
    {
        public string msg { get; set; }
        public string timestamp { get; set; }
        public string formid { get; set; }
        public string uid { get; set; }
        public string fname { get; set; }
        public string id { get; set; }
    }
    public class Adapter_Chat : BaseAdapter<MessageChat>
    {
        Activity context;
        List<MessageChat> list;

        public Adapter_Chat(Activity _context, List<MessageChat> _list)
		:base()
	{
            this.context = _context;
            this.list = _list;
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override MessageChat this[int index]
        {
            get { return list[index]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.item_chat, parent, false);

            MessageChat item = this[position];
            view.FindViewById<TextView>(Resource.Id.time).Text = item.timestamp;
            view.FindViewById<TextView>(Resource.Id.msg).Text = item.msg;
            view.FindViewById<TextView>(Resource.Id.Name).Text = item.fname;
            return view;
        }
    }
}