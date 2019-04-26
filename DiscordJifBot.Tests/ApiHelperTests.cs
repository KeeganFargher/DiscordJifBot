using JiphyLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace DiscordJifBot.Tests
{
    public class ApiHelperTests
    {
        [Fact]
        public void ApiHelper_WhenInstantiated_NotNull()
        {
            ApiHelper.InitializeClient();

            Assert.NotNull(ApiHelper.ApiClient);
            Assert.IsType<HttpClient>(ApiHelper.ApiClient);
        }

        [Fact]
        public void ApiHelper_DefaultRequestHeaders_NotEmpty()
        {
            ApiHelper.InitializeClient();

            Assert.True(ApiHelper.ApiClient.DefaultRequestHeaders.Accept.Count > 0);
        }

        [Fact]
        public void ApiHelper_DefaultRequestHeaders_IsApplicationJson()
        {
            ApiHelper.InitializeClient();
            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");

            Assert.Contains(mediaType, ApiHelper.ApiClient.DefaultRequestHeaders.Accept);
        }
    }
}
