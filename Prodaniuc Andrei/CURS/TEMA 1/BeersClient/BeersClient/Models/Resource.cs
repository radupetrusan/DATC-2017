using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeersClient.Models
{
    public class Resource
    {

        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }

        [JsonProperty("_links")]
        public OtherLinks Links { get; set; }

        public static Resource FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Resource>(json, Converter.Settings);
        }
    }
    public partial class Embedded
    {
        [JsonProperty("brewery")]
        public Brewery[] Brewery { get; set; }
    }

    public class Links
    {
        [JsonProperty("beers")]
        public Beers Beers { get; set; }

        [JsonProperty("self")]
        public Beers Self { get; set; }
    }

    public partial class OtherLinks
    {
        [JsonProperty("brewery")]
        public Beers[] Brewery { get; set; }

        [JsonProperty("self")]
        public Beers Self { get; set; }
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
