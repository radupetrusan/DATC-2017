using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "Application/hal+json");
            string initialPath = "http://datc-rest.azurewebsites.net/breweries";
            var response = client.GetAsync(initialPath).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject<JSONResources>(data);

            List<CBrewery> breweries = obj.homeEmbedded.brewery.ToList();
            foreach (CBrewery brw in breweries)
                Console.WriteLine(brw.breweryId.ToString() + " " + brw.breweryName);
            //Console.WriteLine(data);

            Console.ReadLine();
        }
    }
}
