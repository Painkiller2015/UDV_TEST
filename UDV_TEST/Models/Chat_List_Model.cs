using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using UDV_TEST.DB_Worker;
using UDV_TEST.ViewModels;
using UDV_TEST.Services;
using static Android.Preferences.PreferenceActivity;

namespace UDV_TEST.ViewModels
{
    public class Chat_List_Model : INotifyPropertyChanged
    {
        public List<Chat_List_item> chats = null;
        public event PropertyChangedEventHandler? PropertyChanged;
        public class Chat_List_item
        {
            public int Id { get; set; } 
            public string Header { get; set; }
            public string? Author { get; set; }
            public string? Message { get; set; }
            public string? Date { get; set; }
            public Chat_List_item(int id, string header)
            {
                Id = id;
                Header = header;
            }
            public Chat_List_item(Chat newChat, ChatHistory history)
            {
                Id = newChat.Id;
                Header = newChat.Name;
                Message = history.Message;
                Date = Date_Service.TicksToDateTimeString(history.TimeTicks);
            }

        }
        public void AddNewChat(string chatName)
        {
            using (BaseContext db = new())
            {
                Chat newChat = new() { Name = chatName };
                newChat.Messages.Add(CreateDefaultMessage(newChat));

                db.Chats.Add(newChat);
                db.SaveChanges();

                chats.Add(new Chat_List_item(newChat, newChat.Messages.First()));

                PropertyChanged.Invoke(this, null);
            };
        }
        private ChatHistory CreateDefaultMessage(Chat forChat)
        {
            return new ChatHistory
            {
                Chat = forChat,
                IsBot = true,
                Message = "Привет! Чем могу помочь?",
                TimeTicks = DateTime.Now.Ticks
            };
        }
        public void FillChats()
        {
            using (BaseContext db = new())
            {
                var dbChats = db.Chats.AsNoTracking()
                                .GroupJoin(
                                    db.ChatHistories,
                                    chat => chat.Id,
                                    message => message.Id_Chat,
                                    (chat, messages) => new
                                    {
                                        Id = chat.Id,
                                        Header = chat.Name,
                                        AuthorId = (bool?)messages.OrderByDescending(m => m.TimeTicks).FirstOrDefault().IsBot,
                                        Message = messages.OrderByDescending(m => m.TimeTicks).FirstOrDefault().Message,
                                        Date = (long?)messages.OrderByDescending(m => m.TimeTicks).FirstOrDefault().TimeTicks
                                    }
                                ).ToList();

                chats = dbChats.Select(m => new Chat_List_item(m.Id, m.Header)
                {
                    Author = DB_Service.GetUserNameById(m.AuthorId),
                    Message = m.Message ?? "",
                    Date = Date_Service.TicksToDateTimeString(m.Date ?? DateTime.Now.Ticks)
                }).ToList();
            }
        }
    }
}
