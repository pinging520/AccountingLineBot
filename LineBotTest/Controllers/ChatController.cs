using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace LineBotTest.Controllers
{
    public class ChatController : isRock.LineBot.LineWebHookControllerBase
    {
        //設定ChannelAccessToken
        //const string ChannelAccessToken = "4HSbvWv7uUm99dvbXneBLo92xAI765+7gy6INW7mAHXZ9sFGQsMgrWt3hWo5hOBOxletxgEXTbSRHnAVMeYxqxfl4EOYDE4yeBLNBNwvnNmhQLOo99qcrw49ITahM54BW9HK6HI6OOzsUe134G0SEQdB04t89/1O/w1cDnyilFU=";
        //const string AdminUserId = "U1230c42b9b47725c4c7af17bb646a84d";
        //設定路由https:xxx/Chat
        [Route("Chat")]
        //http Verbs
        [HttpPost] 
        
        public IHttpActionResult POST()
        {
            //string postData = Request.Content.ReadAsStringAsync().Result;
            //var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);

            //抓第1筆
            var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
            try 
            {
                

                string Message="";
                //Message = ReceivedMessage.events[0].message.text;
                if (LineEvent == null)
                { return Ok(); }
                else
                {       //判斷是否訊息 && 訊息格式純文字
                    if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text")
                    {
                        Message =ProcessPostback(LineEvent);
                    }
                     
                }
              


                //isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, Message, ChannelAccessToken);
                this.ReplyMessage(LineEvent.replyToken, Message);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok();
            }
        }

        private string ProcessPostback(isRock.LineBot.Event e)
        {
            const string Error = "格式有誤";
            const string Error1 = "阿不是要省錢?";

            var UserId = e.source.userId;
            var msg = e.message.text;

            //key沒有狀態存在
            if (string.IsNullOrEmpty(Load.GetSet(UserId)))
            {
                //去除空白
                msg = msg.Trim();
                var item = msg.Split(' ');
                //檢查內容
                if (item.Length <= 0) { return Error; }

                int num = 0;
                
                //檢查是否是數值
                if (Int32.TryParse(item[0], out num) == false) { return "輸入的數值有誤"; }

                

                //如果沒問題，保存起來
                Load.Save(UserId + "num", num.ToString());
                

                //回覆QuickReply
                isRock.LineBot.Bot b = new isRock.LineBot.Bot();
                isRock.LineBot.TextMessage TextMessage = new isRock.LineBot.TextMessage($"請選擇或直接輸入這筆金額'{num}'的記帳類別");

                var Types = Load.GetTypes(UserId);
                foreach (var type in Types)
                {
                    TextMessage.quickReply.items.Add(new isRock.LineBot.QuickReplyMessageAction(type, type));
                }
                b.ReplyMessage(e.replyToken, TextMessage);
                //設定狀態
                Load.Save(UserId, "等待分類");
                return "";
            }
            
            //key有分類狀態存在
            if (Load.GetSet(UserId) == "等待分類")
            {
                if (Load.GetSet(UserId + "num") == null)
                {
                    Load.Save(UserId, "");
                    return "請重新輸入金額";
                }

                int num = 0;

                if (Int32.TryParse(Load.GetSet(UserId + "num"), out num) == false)
                {
                    return Error;
                }
                
                //提取種類
                var AccountType = msg.Trim();
                //清空值
                Load.Save(UserId,"");
                if (Load.SaveDB(UserId, num, AccountType))
                {
                    if (num >= 100) { return Error1; }
                    return $"${num} 已記錄為 {AccountType}"; 
                }
                else
                { return Error; }


            }

            

            return Error;
        }

        
        

        
    }
}
