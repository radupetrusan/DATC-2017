using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hal.Client
{
    public class CBreweryEmbedded
    {
        [JsonProperty("_links")]
        public CBrewreyLink links { get; set; }

        [JsonProperty("Id")]
        public int id { get; set; }

        [JsonProperty("Name")]
        public string name { get; set; }
    }

    public class CBrewreyLink
    {
        //beers and self
        [JsonProperty("beers")]
        public EBrwBeers beers { get; set; }

        [JsonProperty("self")]
        public EBrwSelf self { get; set; }

    }

    public class EBrwBeers
    {
        [JsonProperty("href")]
        public string href { get; set; }
    }

    public class EBrwSelf
    {
        [JsonProperty("href")]
        public string href { get; set; }
    }
}
