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
        //
        static public List<string> GetCommandTypes(string UserId)
        {
            List<string> items = new List<string>();
            items.Add("/today");
            items.Add("/month");
            return items;
        }
        //
        public static bool SaveDB(string Userid, int num, string AccountType)
        {
            var _db = new LineBotTest.Models.DbCoNtext();
            var p = new Account { Id = Userid, Price = num, AType = AccountType,DateTime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") };
            _db.Accounts.Add(p);
            _db.SaveChanges();
            return true;

        }

        //本日花費
        public static IQueryable<Account> Day(string c)
        {
            string Date = System.DateTime.Now.ToString("yyyy/MM/dd");
            var _db = new LineBotTest.Models.DbCoNtext();
            IQueryable<Account> query = _db.Accounts.Where(x=> x.Id == c && x.DateTime.Contains(Date));
            return query;
        }
        //本月花費
        public static IQueryable<Account> Month(string c)
        {
            string Date = System.DateTime.Now.ToString("yyyy/MM");
            var _db = new LineBotTest.Models.DbCoNtext();
            IQueryable<Account> query = _db.Accounts.Where(x => x.Id == c && x.DateTime.Contains(Date));
            return query;
        }


    }
}