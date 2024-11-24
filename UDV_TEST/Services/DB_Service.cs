using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDV_TEST.DB_Worker;

namespace UDV_TEST.Services
{
    public static class DB_Service
    {
        public static string GetUserNameById(int id) //идейно - брать из базы
        {
            return id switch
            {
                1 => Application.Context.Resources.GetString(Resource.String.Bot_Name),
                0 => Application.Context.Resources.GetString(Resource.String.User_Name),
                _ => ""
            };
        }
        public static string GetUserNameById(bool? id) 
        {            
            return id switch
            {
                true => Application.Context.Resources.GetString(Resource.String.Bot_Name),
                false => Application.Context.Resources.GetString(Resource.String.User_Name),
                _ => ""
            };
        }
    }
}
