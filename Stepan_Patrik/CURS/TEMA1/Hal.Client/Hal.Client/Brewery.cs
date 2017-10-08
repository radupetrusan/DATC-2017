using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    public class BreweryRef
    {
        [JsonProperty("href")]
        public string href { get; set; }
    }

    public class Brewery
    {
        [JsonProperty("Id")]
        public long id { get; set; }

        [JsonProperty("Name")]
        public string name { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }
    };
}
