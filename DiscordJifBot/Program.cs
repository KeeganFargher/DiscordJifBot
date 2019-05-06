using System;
using System.Reflection;
using System.Threading.Tasks;
using Common;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Giphy;
using Giphy.Models;
using Giphy.Models.Parameters;
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

        private Giphy.Giphy _giphy;

        public async Task RunBotAsync ()
        {
            _keys = new Keys();
            _giphy = new Giphy.Giphy(_keys.GiphyApi);
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
                var context = new SocketCommandContext(_client, message);
                _ = await _commands.ExecuteAsync(context, argPos, _services);

                //  Remove the mention from the message
                var msg = context.Message.Content;
                msg = msg.Split(">")[1];

                //  Call giphy API
                var paramter = new SearchParameter
                {
                    Query = msg
                };
                var rootObject = await _giphy.Search(paramter);

                //  Default reply if there's no data response
                if (rootObject.Data.Count == 0)
                {
                    await message.Channel.SendMessageAsync("I can't find any JIFs for that one :cold_sweat:");
                    return;
                }

                await SendGif(message, context, rootObject);
            }
        }

        private static async Task SendGif(SocketUserMessage message, SocketCommandContext context, RootObject rootObject)
        {
            var r = new Random();
            var randomIndex = r.Next(0, rootObject.Data.Count - 1);
            var item = rootObject.Data[randomIndex];
            var imgUrl = item.Images.Original.Url.OriginalString;

            var embed = new EmbedBuilder()
            {
                Title = item.Title,
                Description = imgUrl,
                Color = Color.DarkBlue,
                ImageUrl = imgUrl
            };

            await message.Channel.SendMessageAsync("", false, embed);

            Console.WriteLine($"Replying to user {context.User.Username} with: { item.Title }");
        }
    }
}