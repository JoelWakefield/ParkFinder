using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ParkFinder.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace ParkFinder
{
    public class ApiHelper
    {
        private readonly IMemoryCache _memoryCache;

        public ApiHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<List<Park>> GetData(string uri)
        {
            List<Park> items = new List<Park>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;

                    items = JsonConvert.DeserializeObject<List<Park>>(result);
                }
            }

            return items;
        }

        public async Task<List<Park>> GetCache(string url)
        {
            string cachKey = "Parks";
            List<Park> parks;

            if (!_memoryCache.TryGetValue(cachKey, out parks))
            {
                //  fetch value
                parks = await GetData(url);

                //  store in the cache
                _memoryCache.Set(
                    cachKey,
                    parks,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                );
            }

            return parks;
        }
    }
}
