using Android.Content;
using UDV_TEST.Adapters;
using static UDV_TEST.ViewModels.Chat_List_Model;
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

            UpdateChatList(true);
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
        public void PopUpAddNewChat()
        {
            newChatName = new(this);

            AlertDialog.Builder dialogBuilder = new(this);
            dialogBuilder?.SetTitle("Новый чат")
                         ?.SetMessage("Название нового чата")
                         ?.SetView(newChatName)
                         ?.SetPositiveButton("добавить", handler: ConfirmButton)
                         ?.SetNegativeButton("отменить", handler: CancelButton);

                         
            AlertDialog dialog = dialogBuilder.Create();
            dialog.SetOnShowListener(new AddChatDialogListener(dialog, newChatName)  );
            dialog.Show();
        }
        private void ConfirmButton(object sender, DialogClickEventArgs e)
        {
            Chat_List.AddNewChat(newChatName.Text);

        }
        private void CancelButton(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Cancel();
        }
        private void UpdateChatList(bool UpdateFromDB = false)
        {
            if (UpdateFromDB)
                Chat_List.FillChats();
            ChatListView.Adapter = new Chat_List_Item_Adapter(this, Chat_List.chats);
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
        private class AddChatDialogListener : Java.Lang.Object, IDialogInterfaceOnShowListener
        {
            private readonly AlertDialog dialog;
            private readonly EditText newChatName;

            public AddChatDialogListener(AlertDialog dialog, EditText input)
            {
                this.dialog = dialog;
                newChatName = input;
            }

            public void OnShow(IDialogInterface dialog)
            {
                Button positiveButton = this.dialog.GetButton((int)DialogButtonType.Positive);
                positiveButton.Enabled = false; 
                
                newChatName.TextChanged += (sender, e) =>
                {
                    string newChatName = this.newChatName.Text ?? "";
                    positiveButton.Enabled = !string.IsNullOrEmpty(newChatName);
                };
            }

        }
    }
}


