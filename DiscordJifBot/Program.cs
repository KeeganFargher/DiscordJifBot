using System;
using System.Reflection;
using System.Threading.Tasks;
using Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using JiphyLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordJifBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;

        private CommandService _commands;

        private IServiceProvider _services;

        private Keys _keys;

        public async Task RunBotAsync ()
        {
            _keys = new Keys();
            _keys.Init();
            ApiHelper.InitializeClient ();
            _client = new DiscordSocketClient ();
            _commands = new CommandService ();

            //  We only want one client running hence singleton pattern
            _services = new ServiceCollection ()
                .AddSingleton (_client)
                .AddSingleton (_commands)
                .BuildServiceProvider ();

            //  Event subscriptions
            _client.Log += Log;

            await RegisterCommandsAsync ();

            await _client.LoginAsync (TokenType.Bot, _keys.BotToken);
            await _client.StartAsync ();

            //  Prevent the application from closing
            await Task.Delay (-1);
        }

        private Task Log (LogMessage arg)
        {
            Console.WriteLine (arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync ()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync (Assembly.GetEntryAssembly ());
        }

        private async Task HandleCommandAsync (SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;

            int argPos = 0;

            bool clientMentionsBot = message.HasMentionPrefix (_client.CurrentUser, ref argPos);

            if (clientMentionsBot)
            {
                var context = new SocketCommandContext (_client, message);
                _ = await _commands.ExecuteAsync(context, argPos, _services);

                //  Remove the mention from the message
                var username = _client.CurrentUser.Mention;
                var msg = context.Message.Content.Replace(username, "");

                //  Call giphy API
                var giphy = new Giphy();
                var rootObject = await giphy.LoadSearch(msg);

                Random r = new Random();
                var randomIndex = r.Next(0, rootObject.Data.Count - 1);
                var item = rootObject.Data[randomIndex];
                var imgUrl = item.Images.Original.Url;

                //var imgUrl = item.Images.Original.Url.OriginalString;

                var embed = new EmbedBuilder()
                {
                    //Title = item.Title,
                    Description = imgUrl,
                    Color = Color.DarkBlue,
                    ImageUrl = imgUrl
                };

                await message.Channel.SendMessageAsync ("", false, embed);

                Console.WriteLine($"Replying to user {context.User.Username} with: { imgUrl }");
            }
        }
    }
}