using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ParkFinder.Models;
using Newtonsoft.Json;

namespace ParkFinder
{
    public class ApiHelper
    {
        public static async Task<List<Park>> GetData(string uri)
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
    }
}
