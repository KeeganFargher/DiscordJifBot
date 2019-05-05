using Common;
using System;
using Xunit;

namespace DiscordJifBot.Tests
{
    public class KeysTest
    {
        private Keys _keys;

        [Fact]
        public void Keys_WhenInstantiated_BotTokenIsNotNullOrEmpty()
        {
            _keys = new Keys();

            var actual = _keys.BotToken;

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
            Assert.NotEmpty(actual);
        }

        [Fact]
        public void Keys_WhenInstantiated_GiphyTokenIsNotNullOrEmpty()
        {
            _keys = new Keys();

            var actual = _keys.GiphyApi;

            Assert.NotNull(actual);
            Assert.IsType<string>(actual);
            Assert.NotEmpty(actual);
        }
    }
}
