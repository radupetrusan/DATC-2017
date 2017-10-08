using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    public class Resource
    {
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        [JsonProperty("_links")]
        public ExtLinks links { get; set; }
    }

    public class ResourceRef
    {
        [JsonProperty("href")]
        public string href { get; set; }
    }

    public class Embedded
    {
        [JsonProperty("brewery")]
        public Brewery[] brewery { get; set; }
    };
}
