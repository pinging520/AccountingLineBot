using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LineBotTest.Models
{
    public class Account
    {
        [Key]
        public int Uid { get; set; }

        public string Id { get; set; }

        public int Price { get; set; }

        public string AType { get; set; }

        public string DateTime { get; set; }
    }
}