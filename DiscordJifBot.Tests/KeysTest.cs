using Common;
using System;
using Xunit;

namespace DiscordJifBot.Tests
{
    public class KeysTest
    {
        [Fact]
        public void GetBotToken_WhenCalled_NotNullOrEmpty()
        {
            var key = Keys.BotToken;

            Assert.NotNull(key);
            Assert.IsType<string>(key);
            Assert.NotEmpty(key);
        }

        [Fact]
        public void GetGiphyApiKey_WhenCalled_NotNullOrEmpty()
        {
            var key = Keys.GiphyApi;

            Assert.NotNull(key);
            Assert.IsType<string>(key);
            Assert.NotEmpty(key);
        }
    }
}
