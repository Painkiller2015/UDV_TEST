﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using UDV_TEST.DB_Worker;
using UDV_TEST.ViewModels;
using UDV_TEST.Services;

namespace UDV_TEST.ViewModels
{
    public class Chat_List_Model : INotifyPropertyChanged
    {
        public List<Chat_List_item> chats = null;
        public event PropertyChangedEventHandler? PropertyChanged;
        public class Chat_List_item
        {
            public required int Id { get; set; }
            public required string Header { get; set; }
            public string? Author { get; set; }
            public string? Message { get; set; }
            public string? Date { get; set; }
        }
        public void AddNewChat(string chatName)
        {
            using (BaseContext db = new())
            {
                Chat newChat = new Chat() { Name = chatName };
                
                db.Chats.Add(newChat);
                db.SaveChanges();
                chats.Add(new Chat_List_item() { Id = newChat.Id, Header = chatName });

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
