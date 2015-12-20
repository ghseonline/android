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
    public class Post
    {
        public string time { get; set; }
        public string msg { get; set; }
    }
    public class NewsAdapter : BaseAdapter<Post>
    {
        Activity context;
        List<Post> list;

        public NewsAdapter(Activity _context, List<Post> _list)
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

        public override Post this[int index]
        {
            get { return list[index]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.item_news, parent, false);

            Post item = this[position];
            view.FindViewById<TextView>(Resource.Id.time).Text = item.time;
            view.FindViewById<TextView>(Resource.Id.msg).Text = item.msg;

            return view;
        }
    }
}