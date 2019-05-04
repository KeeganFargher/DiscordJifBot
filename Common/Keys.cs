using Newtonsoft.Json;
using System;
using System.IO;

namespace Common
{
    public class Keys
    {
        //public static string BotToken = "#{discordtoken}#";
        //public static string GiphyApi = "#{giphytoken}#";

        public string BotToken { get; set; }
        public string GiphyApi { get; set; }

        
        public void Init()
        {
            using (var streamReader = File.OpenText("keys.json"))
            {
                string json = streamReader.ReadToEnd();
                var item = JsonConvert.DeserializeObject<Keys>(json);
                BotToken = item.BotToken;
                GiphyApi = item.GiphyApi;
            }
        }
    }
}
