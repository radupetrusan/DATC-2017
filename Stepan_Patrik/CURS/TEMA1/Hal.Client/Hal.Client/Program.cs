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
			var response = client.GetAsync("http://datc-rest.azurewebsites.net/beers").Result;

			var data = response.Content.ReadAsStringAsync().Result;
			var obj = JsonConvert.DeserializeObject(data);

			Console.WriteLine(data);

			Console.ReadLine();
		}
	}
}
