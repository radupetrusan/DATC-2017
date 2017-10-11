using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hal.Client
{
    public class JSONResources
    {
        [JsonProperty("_embedded")]
        public Embedded embedded { get; set; }

        [JsonProperty("_links")]
        public Links links { get; set; }

    }

    public partial class Embedded
    {
        [JsonProperty("brewery")]
        public CBreweryEmbedded[] brewery { get; set; }
    }

    public class Links
    {
        [JsonProperty("brewery")]
        public List<LBrwHref> brewery { get; set; }

        [JsonProperty("self")]
        public LinkSelf selfLink { get; set; }
    }

    public class LBrwHref
    {
        [JsonProperty("href")]
        public string hrefBrewery { get; set; }

    }

    public class LinkSelf
    {
        [JsonProperty("href")]
        public string hrefSelf { get; set; }
    }
}
