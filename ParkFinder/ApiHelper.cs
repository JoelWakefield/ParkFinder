using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ParkFinder.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ParkFinder
{
    public class ApiHelper
    {
        private readonly ILogger<ApiHelper> _logger;
        private readonly IMemoryCache _memoryCache;

        private readonly string ApiUrl = "https://seriouslyfundata.azurewebsites.net/api/parks";
        private readonly string cacheKey = "parks";

        public ApiHelper(ILogger<ApiHelper> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        private async Task<List<Park>> GetData()
        {
            List<Park> items = new List<Park>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiUrl);

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    items = JsonConvert.DeserializeObject<List<Park>>(result);

                    _logger.LogInformation("successful pull from api");
                }
                else
                {
                    _logger.LogInformation("failure to pull from api");
                }
            }

            return items;
        }

        public async Task<List<Park>> Get(string search = null)
        {
            List<Park> parks;

            if (!_memoryCache.TryGetValue(cacheKey, out parks))
            {
                //  fetch value
                parks = await GetData();

                //  store in the cache
                _memoryCache.Set(
                    cacheKey,
                    parks,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
                );

                _logger.LogInformation($"cache {cacheKey} being filled");
            }
            else
            {
                _logger.LogInformation($"cache {cacheKey} already full");
            }

            //  filter out results
            if (search != null)
            {
                _logger.LogInformation("returning filtered data");

                return parks.Where(p =>
                {
                    return p.ParkName.Contains(search) || p.Description.Contains(search);
                }).ToList();
            }
            else
            {
                _logger.LogInformation("returning all data");
                return parks;
            }
        }
    }
}
