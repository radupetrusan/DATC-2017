using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hal.Client
{
    public class BeerFef
    {
        [JsonProperty("href")]
        public string href { get; set; }
    };
}
