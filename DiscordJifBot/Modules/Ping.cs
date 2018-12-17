using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using JiphyLibrary;

namespace DiscordJifBot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {

        [Command("wholesome")]
        public async Task PingAsync()
        {
            ApiHelper.InitializeClient();

            var rootObject = await SearchProcessor.LoadSearch("fast");

            Console.WriteLine(rootObject.Data[0].Images.Original.Url);

            await ReplyAsync(rootObject.Data[0].Images.Original.Url);
        }
    }

}