using Android.Content;
using Android.Views;
using static UDV_TEST.ViewModels.Chat_Model;

namespace UDV_TEST.Adapters
{
    public class Chat_Message_Adapter : BaseAdapter<Chat_Message>
    {
        private readonly List<Chat_Message> items;
        private readonly Context context;
        public Chat_Message_Adapter(Context context, List<Chat_Message> items)
        {
            this.context = context;
            this.items = items;
        }

        public override Chat_Message this[int position] => items[position];
        public override int Count => items.Count;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? LayoutInflater.From(context).Inflate(Resource.Layout.Chat_Message, parent, false);


            var AuthorNameTextView = view.FindViewById<TextView>(Resource.Id.AuthorName);
            var MessageTextView = view.FindViewById<TextView>(Resource.Id.Message);
            var MessageDateTextView = view.FindViewById<TextView>(Resource.Id.Date);

            AuthorNameTextView.Text = items[position].AuthorName;
            MessageTextView.Text = items[position].Message;
            MessageDateTextView.Text = items[position].Date;

            return view;
        }
    }
}