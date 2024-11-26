using Android.Content;
using UDV_TEST.Adapters;
using UDV_TEST.ViewModels;
using static UDV_TEST.ViewModels.Chat_Model;

namespace UDV_TEST
{
    [Activity(Label = "@string/app_name")]
    internal class ChatDetailActivity : Activity
    {
        public Chat_Model chat = null;
        private int chatId;
        private EditText newMessage = null;
        private ListView chatMessagesView = null;
        private Button btnSend = null;
        private Button btnGoBack = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitLocalComponents();
            SubscribeToEvents();
        }
        private void InitLocalComponents()
        {
            SetContentView(Resource.Layout.Chat);

            chatId = Intent.GetIntExtra("ChatId", 0);
            chat = new(chatId);

            chatMessagesView = FindViewById<ListView>(Resource.Id.MessagesList);
            btnGoBack = FindViewById<Button>(Resource.Id.BtnBack);
            newMessage = FindViewById<EditText>(Resource.Id.NewMessage);
            btnSend = FindViewById<Button>(Resource.Id.BtnSendMessage);

            UpdateChatList(true);
        }
        private void SubscribeToEvents()
        {
            chat.PropertyChanged += (sender, e) =>
            {
                UpdateChatList();
            };
            btnGoBack.Click += (sender, e) =>
            {
                GoBack();
            };
            btnSend.Click += (sender, e) =>
            {
                ProcessingMessage();
            };
        }

        private void GoBack()
        {
            Intent intent = new(this, typeof(MainActivity));
            StartActivity(intent);
        }
        private void ProcessingMessage()
        {
            if (string.IsNullOrWhiteSpace(newMessage.Text))
                return;

            Chat_Message message = new(false, newMessage.Text, DateTime.Now);
            chat.SendMessage(message);
        }
        private void UpdateChatList(bool UpdateFromDB = false)
        {
            if (UpdateFromDB)
                chat.FillChat();
            chatMessagesView.Adapter = new Chat_Message_Adapter(this, chat.messages);
        }
    }
}
