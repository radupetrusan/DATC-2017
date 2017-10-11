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
            var response = client.GetAsync("http://datc-rest.azurewebsites.net/breweries").Result;

			var data = response.Content.ReadAsStringAsync().Result;
			var obj = JsonConvert.DeserializeObject<JSONResources>(data);

            List<CBreweryEmbedded> breweries = obj.embedded.brewery.ToList();
            foreach (CBreweryEmbedded brw in breweries)
                Console.WriteLine(brw.id.ToString()+ " " + brw.name);
			//Console.WriteLine(data);

			Console.ReadLine();
		}
	}
}
