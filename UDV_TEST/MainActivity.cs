using Android.Content;
using UDV_TEST.Adapters;
using UDV_TEST.DB_Worker;
using UDV_TEST.Entity;
using UDV_TEST.Services;
using UDV_TEST.ViewModels;

namespace UDV_TEST
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public static Chat_List_Model Chat_List { get; } = new();        
        private ListView ChatListView = null;
        private Button BtnAddNewChat = null;
        private EditText newChatName = null;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            InitLocalComponents();
            SubscribeToEvents();
        }
        private void InitLocalComponents()
        {            
            SetContentView(Resource.Layout.Chat_list);

            BtnAddNewChat = FindViewById<Button>(Resource.Id.BtnAddNewChat);
            ChatListView = FindViewById<ListView>(Resource.Id.ChatList);

            UpdateChatList();
        }
        private void SubscribeToEvents()
        {
            BtnAddNewChat.Click += (sender, e) =>
            {
                PopUpAddNewChat();
            };

            Chat_List.PropertyChanged += (sender, e) =>
            {
                UpdateChatList();
            };

            ChatListView.ItemClick += (sender, e) =>
            {
                GoToChat(e.Position);
            };
        }

        private void GoToChat(int elPosition)
        {
            var selectedItem = ChatListView.Adapter.GetItem(elPosition);
            Chat_List_item chat = Java_Service.CastJavaObject.Cast<Chat_List_item>(selectedItem);
            int selectedChatId = chat.Id;

            Intent intent = new(this, typeof(ChatDetailActivity));
            intent.PutExtra("ChatId", selectedChatId);

            StartActivity(intent);
        }
        
        public void PopUpAddNewChat()
        {
            newChatName = new EditText(this);

            AlertDialog.Builder dialogBuilder = new(this);
            dialogBuilder?.SetTitle("Новый чат")
                         ?.SetMessage("Название нового чата")
                         ?.SetView(newChatName)
                         ?.SetPositiveButton("добавить", handler: ConfirmButton)
                         ?.SetNegativeButton("отменить", handler: CancelButton);
            AlertDialog dialog = dialogBuilder.Create();
            dialog.Show();
        }

        private void ConfirmButton(object sender, DialogClickEventArgs e)
        {
            string newChatName = this.newChatName.Text ?? "";
            Chat_List.AddNewChat(newChatName);
        }
        private void CancelButton(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Cancel();
        }

        private void UpdateChatList()
        {
            Chat_List.FillChats();            
            ChatListView.Adapter = new Chat_List_Item_Adapter(this, Chat_List.chats);            
        }
    }
}


