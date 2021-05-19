using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LineBotTest.Models;
using System.Data.Entity;

namespace LineBotTest
{
    public class Load
    {

        public static string GetSet(string key)
        {
            if (System.Web.HttpContext.Current.Application[key] == null)
            { return null; }
            return System.Web.HttpContext.Current.Application[key].ToString();      
        }

        public static void Save(string key, string num) 
        {
            System.Web.HttpContext.Current.Application[key] = num;
        }

        static public List<string> GetTypes(string UserId)
        {
            List<string> items = new List<string>();
            items.Add("餐費");
            items.Add("交通費");
            items.Add("娛樂費");
            items.Add("服裝費");
            return items;
        }
        
        public static bool SaveDB(string Userid, int num, string AccountType)
        {
            var _db = new LineBotTest.Models.DbCoNtext();
            var p = new Account { Id = Userid, Price = num, AType = AccountType };
            _db.Accounts.Add(p);
            _db.SaveChanges();
            return true;

        }
        

    }
}