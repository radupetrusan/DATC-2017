﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;




namespace Hal.Client
{

    class Berarii
    {
        public string id { get; set; }
        public string name { get; set; }
        public string links { get; set; }
    }

    class ListaBerarii
    {
        public List<Berarii> ResourceList = new List<Berarii>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://datc-rest.azurewebsites.net/breweries").Result;

            var data = response.Content.ReadAsStringAsync().Result;

            //var obj = JsonConvert.DeserializeObject(data);
            HttpRequestHeaders req = response.RequestMessage.Headers;
            req.Add("Accept", "application/hal+json");
            //Console.WriteLine(obj);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ListaBerarii lista = (ListaBerarii)serializer.Deserialize(data, typeof(ListaBerarii));

            //Console.WriteLine(data);

            foreach (Berarii res in lista.ResourceList)
            {
                Console.WriteLine("ID:"+ res.id);
                Console.WriteLine("Name:" + res.name);
                Console.WriteLine("Links:" + res.links);
            }

           string strserial =  serializer.Serialize(lista);
            Console.WriteLine(strserial);

            Console.ReadLine();
        }
    }
}
