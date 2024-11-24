using Android.Content;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDV_TEST.DB_Worker;
using UDV_TEST.Entity;
using UDV_TEST.Services;

namespace UDV_TEST.ViewModels
{
    public class Chat_List_Model : INotifyPropertyChanged
    {        
        public List<Chat_List_item> chats = null;
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public void AddNewChat(string chatName)
        {
            using (BaseContext db = new())
            {
                Chat newChat = new Chat() { Name = chatName };

                chats.Add(new Chat_List_item() { Id = newChat.Id, Header = chatName });
                db.Chats.Add(newChat);               
                db.SaveChanges();                

                PropertyChanged.Invoke(this, null);
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
                
                chats = dbChats.Select(m => new Chat_List_item()
                {
                    Id = m.Id,
                    Author = DB_Service.GetUserNameById(m.AuthorId),
                    Header = m.Header,
                    Message = m.Message ?? "",                    
                    Date = Date_Service.TicksToDateTimeString(m.Date)
                }).ToList();
            }
        }
    }
}
