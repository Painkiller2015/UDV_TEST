using Android.Content;
using Android.Views;
using UDV_TEST.Entity;

namespace UDV_TEST.Adapters
{
    public class Chat_List_Item_Adapter : BaseAdapter<Chat_List_item>
    {
        private readonly List<Chat_List_item> items;
        private readonly Context context;

        public Chat_List_Item_Adapter(Context context, List<Chat_List_item> items)
        {
            this.context = context;
            this.items = items;
        }

        public override Chat_List_item this[int position] => items[position];
        public override int Count => items.Count;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.From(context).Inflate(Resource.Layout.Chat_list_Item, parent, false);
            

            var HeaderTextView = view.FindViewById<TextView>(Resource.Id.Header);
            var AuthorNameTextView = view.FindViewById<TextView>(Resource.Id.AuthorName);
            var LastMessageTextView = view.FindViewById<TextView>(Resource.Id.LastMessage);
            var MessageDateTextView = view.FindViewById<TextView>(Resource.Id.Date);

            HeaderTextView.Text = items[position].Header;
            AuthorNameTextView.Text = items[position].Author;
            LastMessageTextView.Text = items[position].Message;
            MessageDateTextView.Text = items[position].Date;

            return view;
        }
    }
}