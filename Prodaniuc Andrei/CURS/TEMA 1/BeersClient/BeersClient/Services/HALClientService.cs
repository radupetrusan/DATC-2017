using BeersClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BeersClient.Services
{
    public class HALClientService
    {
        public static string _homeBaseUrl = "http://datc-rest.azurewebsites.net/";
        HttpClient _client;
        public HALClientService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_homeBaseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/hal+json");

        }
        public List<Brewery> GetBreweries()
        {
            var response = _client.GetAsync("breweries/").Result;
            var jsonString=response.Content.ReadAsStringAsync().Result;
            var resource = JsonConvert.DeserializeObject<Resource>(jsonString);
            return resource.Embedded.Brewery.ToList();
        }

        public Brewery GetBrewery(string brewery)
        {
            var response = _client.GetAsync(brewery).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Brewery>(jsonString);
        }

        internal List<Beer> GetBeers(string beers)
        {
            var response = _client.GetAsync(beers).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Beer>>(jsonString);
        }
        public Beer GetBeer(string beer)
        {
            var response = _client.GetAsync(beer).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Beer>(jsonString);
        }
    }
}
