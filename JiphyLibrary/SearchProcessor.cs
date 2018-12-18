﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common;
using JiphyLibrary.Models;

namespace JiphyLibrary
{
    public class SearchProcessor
    {
        public static async Task<RootObject> LoadSearch(string search)
        {
            int limit = 30;
            int offset = 0;
            string rating = "y";
            string language = "en";

            string uri = $"https://api.giphy.com/v1/gifs/search?" +
                         $"api_key={ Keys.GiphyApi }&" +
                         $"q={ Uri.EscapeUriString(search) }&" +
                         $"limit={ limit }&" +
                         $"offset={ offset }&" +
                         $"lang={ language }";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(uri))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.ReasonPhrase);
                };

                var rootObject = await response.Content.ReadAsAsync<RootObject>();
                return rootObject;
            }

        }
    }
}
