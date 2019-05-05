using Newtonsoft.Json;
using System;
using System.IO;

namespace Common
{

    public class Keys
    {
        public string BotToken { get; set; }
        public string GiphyApi { get; set; }

        public Keys()
        {
            Init();
        }
       
        private void Init()
        {
            using (var streamReader = File.OpenText("keys.json"))
            {
                string json = streamReader.ReadToEnd();
                dynamic result = JsonConvert.DeserializeObject(json);
                BotToken = result.BotToken;
                GiphyApi = result.GiphyApi;
            }
        }
    }
}
