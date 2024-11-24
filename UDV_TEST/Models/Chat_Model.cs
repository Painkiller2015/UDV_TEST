using BotService;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using UDV_TEST.DB_Worker;
using UDV_TEST.Services;

namespace UDV_TEST.ViewModels
{
    public class Chat_Model : INotifyPropertyChanged
    {
        public int Id { get; init; }
        public List<Chat_Message> messages { get; set; } = [];
        public event PropertyChangedEventHandler? PropertyChanged;
        public Chat_Model(int id)
        {
            Id = id;
        }
        public class Chat_Message
        {
            public string AuthorName { get; init; }
            public string Message { get; set; }
            public long TimeTicks { get; init; }
            public string Date { get; init; }
            public bool IsBot { get; init; }
            public Chat_Message(bool isBot, string message, DateTime dateTime)
            {
                Message = message;
                TimeTicks = dateTime.Ticks;
                Date = Date_Service.DateTimeToString(dateTime);
                IsBot = isBot;
                AuthorName = DB_Service.GetUserNameById(isBot);
            }        
        }
        public void FillChat()
        {
            using (BaseContext db = new())
            {
                messages = db.ChatHistories.AsNoTracking()
                                            .Where(chat => chat.Id_Chat == this.Id)
                                            .OrderBy(chat => chat.TimeTicks)
                                            .Select(chat => new Chat_Message(chat.IsBot, chat.Message, new DateTime(chat.TimeTicks))
                                            ).ToList();
            }
        }
        public void SendMessage(Chat_Message message)
        {
            AddMessage(message);
            GetAnswer(message);
        } 
        private void GetAnswer(Chat_Message message)
        {
            BOT.Answer answer = BOT.SendMessage(message.Message);

            if (answer != null)
            {
                AddMessage(true, answer.Message, answer.Date);
            }
        }
        private void AddMessage(bool isBot, string message, DateTime date)
        {
            Chat_Message newMessage = new(isBot, message, date);
            messages.Add(newMessage);
            SaveMessage(newMessage);
        }
        private void AddMessage(Chat_Message message)
        {
            messages.Add(message);
            SaveMessage(message);
        }
        private void SaveMessage(Chat_Message cm)
        {
            ChatHistory history = new()
            {
                Id_Chat = Id,
                IsBot = cm.IsBot,
                Message = cm.Message,
                TimeTicks = cm.TimeTicks
            };

            using (BaseContext db = new())
            {
                db.ChatHistories.Add(history);
                db.SaveChanges();
            }
            PropertyChanged.Invoke(this, null);
        }
    }
}
